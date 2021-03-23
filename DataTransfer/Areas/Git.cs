using DataTransfer.Context;
using DataTransfer.Enums;
using DataTransfer.Helper;
using DataTransfer.Model.Api;
using DataTransfer.Model.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer.Areas
{
    public class Git: IDisposable
    {
        private Utils _utils;
        private UriHelper _uriHelper;
        private string _credentials;
        private bool disposedValue;
        private bool disposedValue1;

        public Git()
        {
            _utils = new Utils();
            _uriHelper = new UriHelper();
            _credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", Environment.GetEnvironmentVariable("PersonalAccessToken"))));

        }

        public async Task<int> ListAllRepositoriesAsync()
        {
            
            // verificar ultima atualização bem sucedida
            // se foi hoje, não precisa executar
            if (_utils.AlreadyBeenExecuted(Methods.Repositories))
            {
                await _utils.SaveLoadHistory(Methods.Repositories, 0, true, "load has already been successfully executed");
                return 0;
            }

            int ret = 0;

            var _uri = _uriHelper.Get(Methods.Repositories, new UriParameters());
            
            HttpClientHelper request = new HttpClientHelper(_credentials, _uri);
            var resp = await request.GetAsync();
            string json = resp.Content;

            RepositoryReference.Root repos = JsonConvert.DeserializeObject<RepositoryReference.Root>(json);

            var _fromApi = repos.value.Select(r => new RepositoryModel()
            {
                DefaultBranch = r.defaultBranch,
                RepositoryId = r.id,
                IsActive = true,
                LoadDate = DateTime.Now,
                Name = r.name,
                ProjectId = r.project.id,
                RemoteUrl = r.remoteUrl,
                Size = r.size,
                SshUrl = r.sshUrl,
                Url = r.url,
                WebUrl = r.webUrl
            }).ToList();

            using (DbDevOpsDashContext ctx = new DbDevOpsDashContext())
            {
                var _inDb = await ctx.Repositories.Where(r => r.IsActive == true).AsNoTracking().ToListAsync();

                // get ID from Db
                foreach (var inDb in _inDb)
                {
                    if (_fromApi.Where(fromApi => fromApi.ProjectId == inDb.ProjectId && fromApi.RepositoryId == inDb.RepositoryId).Any())
                        _fromApi
                            .Where(fromApi => fromApi.ProjectId == inDb.ProjectId && fromApi.RepositoryId == inDb.RepositoryId)
                            .FirstOrDefault().Id = inDb.Id;
                }

                // to update
                var _toUpdate = _fromApi
                    .Intersect(_inDb, new RepositoryEqualityComparer())
                    .Select(j => new RepositoryModel
                    {
                        Id = j.Id,
                        Name = j.Name,
                        Url = j.Url,
                        LoadDate = DateTime.Now,
                        IsActive = j.IsActive,
                        DefaultBranch = j.DefaultBranch,
                        ProjectId = j.ProjectId,
                        RemoteUrl = j.RemoteUrl,
                        Size = j.Size,
                        SshUrl = j.SshUrl,
                        WebUrl = j.WebUrl,
                        RepositoryId = j.RepositoryId,
                    });

                ret = _toUpdate.Count();
                ctx.Repositories.UpdateRange(_toUpdate);

                // to delete
                var _toDelete = _inDb.Where(x => !_fromApi.Any(y => y.RepositoryId == x.RepositoryId && y.ProjectId == x.ProjectId)).ToList();
                _toDelete.ToList().ForEach(x => { x.IsActive = false; x.LoadDate = DateTime.Now; });
                ret = ret + _toDelete.Count;
                ctx.Repositories.UpdateRange(_toDelete);

                // to add
                var _toAdd = _fromApi.Where(x => !_inDb.Any(y => y.RepositoryId == x.RepositoryId && y.ProjectId == x.ProjectId)).ToList();
                await ctx.Repositories.AddRangeAsync(_toAdd);
                ret = ret + _toAdd.Count;

                // save history
                ctx.LoadHistory.Add(_utils.LoadHistoryHelper(Methods.Repositories, ret, true, ""));

                // commit
                await ctx.SaveChangesAsync();

            }
            return ret;
        }

        public async Task<int> ListAllCommitsAsync()
        {
            int ret = 0;
            DateTime maxToDate = DateTime.Today;
            DateTime minFromDate = Convert.ToDateTime(Environment.GetEnvironmentVariable("MinFromDate").ToString());
            int dateIncrement = Convert.ToInt32(Environment.GetEnvironmentVariable("DateIncrement").ToString());

            //searchCriteria.fromDate - If provided, only include history entries created after this date(string)
            DateTime fromDate = _utils.GetLastToDate(Methods.Commits);

            // se fromDate = minvalue -> executar carga inicial a partir de 01/01/2020 até data de hoje
            if (fromDate >= DateTime.Today)
            {
                await _utils.SaveLoadHistory(Methods.Commits, ret, true, "load has already been successfully executed");
                return 0;
            }

            //repos list
            DbDevOpsDashContext reposCtx = new DbDevOpsDashContext();
            var repos = await reposCtx.Repositories.Where(r => r.IsActive == true).AsNoTracking().ToListAsync();
            await reposCtx.DisposeAsync();

            //searchCriteria.fromDate - If provided, only include history entries created after this date(string)
            if (fromDate < minFromDate)
                fromDate = minFromDate;

            //searchCriteria.toDate   - If provided, only include history entries created before this date(string)
            DateTime toDate = fromDate.AddDays(dateIncrement);

            if (toDate > maxToDate)
                toDate = maxToDate;

            while (true)
            {
                List<CommitModel> _fromApi = new List<CommitModel>();

                using (DbDevOpsDashContext ctx = new DbDevOpsDashContext())
                {
                    foreach (var r in repos)
                    {
                        var commits = await GetAllCommitsByRepositoryId(r.RepositoryId, fromDate, toDate);

                        var _tmpApi = commits.value.Select(v => new CommitModel()
                        {
                            CommitId = v.commitId,
                            RepositoryId = r.RepositoryId,
                            AuthorDate = v.author.date,
                            AuthorEmail = v.author.email,
                            AuthorName = v.author.name,
                            ChangeCountsAdd = v.changeCounts.Add,
                            ChangeCountsDelete = v.changeCounts.Delete,
                            ChangeCountsEdit = v.changeCounts.Edit,
                            Comment = v.comment,
                            CommentTruncated = v.commentTruncated,
                            CommitterDate = v.committer.date,
                            CommitterEmail = v.committer.email,
                            CommitterName = v.committer.name,
                            RemoteUrl = v.remoteUrl,
                            Url = v.url,
                            LoadDate = toDate,
                            IsActive = true,
                        }).ToList();

                        _fromApi.AddRange(_tmpApi);

                    }

                    // add
                    await ctx.Commits.AddRangeAsync(_fromApi);

                    // history
                    await ctx.LoadHistory.AddAsync(_utils.LoadHistoryHelper(Methods.Commits, _fromApi.Count, true, "", fromDate, toDate));

                    // save
                    await ctx.SaveChangesAsync();

                }

                ret += _fromApi.Count;

                // controle
                if (toDate >= maxToDate)
                    break;

                fromDate = toDate;
                toDate = fromDate.AddDays(dateIncrement);

                if (toDate > maxToDate)
                    toDate = maxToDate;

            }

            return ret;
        }

        private async Task<CommitReference.Root> GetAllCommitsByRepositoryId(string repositoryId, DateTime fromDate, DateTime toDate)
        {

            var _uri = _uriHelper.Get(Methods.Commits,
                new UriParameters()
                {
                    repositoryId = repositoryId,
                    fromDate = fromDate.ToString("yyyy-MM-dd"),
                    toDate = toDate.ToString("yyyy-MM-dd"),
                });

            HttpClientHelper request = new HttpClientHelper(_credentials, _uri);
            var resp = await request.GetAsync();
            string json = resp.Content;

            CommitReference.Root commits = JsonConvert.DeserializeObject<CommitReference.Root>(json);
            return commits;

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue1)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue1 = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Git()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
