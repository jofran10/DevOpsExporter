using DataTransfer.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DataTransfer.Helper;
using DataTransfer.Model.Api;
using Newtonsoft.Json;
using System.Linq;
using DataTransfer.Model.Data;
using DataTransfer.Context;
using Microsoft.EntityFrameworkCore;

namespace DataTransfer.Areas
{
    public class Projects : IDisposable
    {
        private Utils _utils;
        private UriHelper _uriHelper;
        private string _credentials;
        private bool disposedValue;

        public Projects()
        {
            _utils = new Utils();
            _uriHelper = new UriHelper();
            _credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", Environment.GetEnvironmentVariable("PersonalAccessToken"))));

        }

        public async Task<int> ListAllAsync()
        {
            // verificar ultima atualização bem sucedida -> se foi hoje, não precisa executar
            if (_utils.AlreadyBeenExecuted(Methods.Projects))
            {
                await _utils.SaveLoadHistory(Methods.Projects, 0, true, "load has already been successfully executed");
                return 0;
            }

            int ret = 0;
            var _uri = _uriHelper.Get(Methods.Projects, new UriParameters());

            HttpClientHelper request = new HttpClientHelper(_credentials, _uri);
            var resp = await request.GetAsync();
            string json = resp.Content;

            ProjectReference.Root projects = JsonConvert.DeserializeObject<ProjectReference.Root>(json);

            var _fromApi = projects.value.Select(p => new ProjectModel()
            {
                Description = p.description,
                ProjectId = p.id,
                LastUpdateTime = p.lastUpdateTime,
                Name = p.name,
                Revision = p.revision,
                State = p.state,
                Url = p.url,
                Visibility = p.visibility,
                IsActive = true,
                LoadDate = DateTime.Now
            }).ToList();

            using (DbDevOpsDashContext ctx = new DbDevOpsDashContext())
            {
                var _inDb = await ctx.Projects.Where(p => p.IsActive == true).AsNoTracking().ToListAsync();

                // get ID from Db
                foreach (var inDb in _inDb)
                {
                    if (_fromApi.Where(fromApi => fromApi.ProjectId == inDb.ProjectId).Any())
                        _fromApi.Where(fromApi => fromApi.ProjectId == inDb.ProjectId).FirstOrDefault().Id = inDb.Id;
                }

                // projs to update
                var _toUpdate = _fromApi
                    .Intersect(_inDb, new ProjectEqualityComparer())
                    .Select(j => new ProjectModel
                    {
                        Id = j.Id,
                        Description = j.Description,
                        LastUpdateTime = j.LastUpdateTime,
                        Name = j.Name,
                        Revision = j.Revision,
                        State = j.State,
                        Url = j.Url,
                        Visibility = j.Visibility,
                        LoadDate = DateTime.Now,
                        IsActive = j.IsActive,
                        ProjectId = j.ProjectId,
                    });

                ret = _toUpdate.Count();
                ctx.Projects.UpdateRange(_toUpdate);

                // projs to delete
                var _toDelete = _inDb.Where(x => !_fromApi.Any(y => y.ProjectId == x.ProjectId)).ToList();
                _toDelete.ToList().ForEach(x => { x.IsActive = false; x.LoadDate = DateTime.Now; });
                ret = ret + _toDelete.Count;
                ctx.Projects.UpdateRange(_toDelete);

                // projs to add
                var _toAdd = _fromApi.Where(x => !_inDb.Any(y => y.ProjectId == x.ProjectId)).ToList();
                await ctx.Projects.AddRangeAsync(_toAdd);
                ret = ret + _toAdd.Count;

                // save history
                ctx.LoadHistory.Add(_utils.LoadHistoryHelper(Methods.Projects, ret, true, ""));

                // commit
                await ctx.SaveChangesAsync();

            }

            return ret;

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Projects()
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
