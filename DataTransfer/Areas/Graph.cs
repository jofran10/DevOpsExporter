using DataTransfer.Context;
using DataTransfer.Enums;
using DataTransfer.Extensions.Enums;
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
    public class Graph : IDisposable
    {
        private Utils _utils;
        private UriHelper _uriHelper;
        private string _credentials;
        private bool disposedValue;

        public Graph()
        {
            _utils = new Utils();
            _uriHelper = new UriHelper();
            _credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", Environment.GetEnvironmentVariable("PersonalAccessToken"))));

        }

        public async Task<string> GetDescriptorAsync(string storageKey)
        {
            var _uri = _uriHelper.Get(Methods.Descriptor,
                new UriParameters()
                {
                   storageKey = storageKey,
                });

            HttpClientHelper request = new HttpClientHelper(_credentials, _uri);
            var resp = await request.GetAsync();
            string json = resp.Content;

            DescriptorReference.Root descriptor = JsonConvert.DeserializeObject<DescriptorReference.Root>(json);
            return descriptor.value;
        }

        public async Task<string> GetStorageKeyAsync(string subjectDescriptor)
        {
            var _uri = _uriHelper.Get(Methods.Descriptor,
               new UriParameters()
               {
                   subjectDescriptor = subjectDescriptor,
               });

            HttpClientHelper request = new HttpClientHelper(_credentials, _uri);
            var resp = await request.GetAsync();
            string json = resp.Content;

            DescriptorReference.Root storageKey = JsonConvert.DeserializeObject<DescriptorReference.Root>(json);
            return storageKey.value;
        }

        public async Task<int> ListAllGroupsAsync()
        {
            int ret = 0;
            string restoreKey = "TbGroups.Id";

            if (_utils.AlreadyBeenExecuted(Methods.Groups))
            {
                await _utils.SaveLoadHistory(Methods.Groups, ret, true, "load has already been successfully executed");
                return 0;
            }

            // all projs
            string aux;
            int intPId = 0;

            var restorePoint = _utils.GetRestorePoint(Methods.Groups, DateTime.Today);
            if (restorePoint.Count > 0)
            {
                restorePoint.TryGetValue(restoreKey, out aux);
                intPId = Convert.ToInt32(aux);
            }

            DbDevOpsDashContext projsCtx = new DbDevOpsDashContext();
            var projects = await projsCtx.Projects.AsNoTracking().Where(p => p.Id > intPId && p.IsActive == true).OrderBy(p => p.Id).ToListAsync();
            await projsCtx.DisposeAsync();

            // all pipelines
            foreach (var proj in projects)
            {

                var _fromApi = await GetAllGroupsByProjectIdAsync(proj.ProjectId);
                if (_fromApi.Count <= 0)
                    continue;
                                

                using (DbDevOpsDashContext ctx = new DbDevOpsDashContext())
                {
                    var _inDb = await ctx.Groups
                            .Where(p => p.ProjectId == proj.ProjectId && p.IsActive == true)
                            .AsNoTracking()
                            .ToListAsync();

                    // get ID from Db
                    foreach (var inDb in _inDb)
                    {
                        if (_fromApi.Where(fromApi => fromApi.ProjectId == inDb.ProjectId && fromApi.OriginId == inDb.OriginId).Any())
                            _fromApi.Where(fromApi => fromApi.ProjectId == inDb.ProjectId && fromApi.OriginId == inDb.OriginId)
                                .FirstOrDefault().Id = inDb.Id;
                    }

                    // to update
                    var _toUpdate = _fromApi
                        .Intersect(_inDb, new GroupEqualityComparer())
                        .Select(r => new GroupModel
                        {
                            IsActive = r.IsActive,
                            LoadDate = DateTime.Now,
                            ProjectId = proj.ProjectId,
                            Url = r.Url,
                            Id = r.Id,
                            Descriptor = r.Descriptor,
                            DisplayName = r.DisplayName,
                            Domain = r.Domain,
                            MailAddress = r.MailAddress,
                            PrincipalName = r.PrincipalName,
                            Origin = r.Origin,
                            OriginId = r.OriginId,
                            OurGroupCode = _utils.GetOurGroupCode(r.DisplayName),

                        }); ;

                    ret = ret + _toUpdate.Count();
                    ctx.Groups.UpdateRange(_toUpdate);

                    // to delete
                    var _toDelete = _inDb.Where(inDb => !_fromApi.Any(fromApi => fromApi.ProjectId == inDb.ProjectId && fromApi.OriginId == inDb.OriginId)).ToList();
                    _toDelete.ToList().ForEach(x => { x.IsActive = false; x.LoadDate = DateTime.Now; });
                    ret = ret + _toDelete.Count;
                    ctx.Groups.UpdateRange(_toDelete);

                    // to add
                    var _toAdd = _fromApi.Where(fromApi => !_inDb.Any(inDb => fromApi.ProjectId == inDb.ProjectId && fromApi.OriginId == inDb.OriginId)).ToList();
                    await ctx.Groups.AddRangeAsync(_toAdd);
                    ret = ret + _toAdd.Count;

                    // set OurGroupCode
                    _toAdd.ForEach(x => { x.OurGroupCode = _utils.GetOurGroupCode(x.DisplayName); });

                    // restore point
                    var rpm = new LoadRestoreModel()
                    {
                        ExecutionDateTime = DateTime.Now,
                        LastId = $"{restoreKey}={proj.Id}",
                        LoadDate = DateTime.Today,
                        Method = (int)Methods.Groups,
                        Id = (int)Methods.Groups,
                    };

                    ctx.LoadRestorePoint.Update(rpm);

                    // commit
                    await ctx.SaveChangesAsync();

                }

            }

            await _utils.SaveLoadHistory(Methods.Groups, ret, true, "");
            return ret;

        }


        private async Task<List<GroupModel>> GetAllGroupsByProjectIdAsync(string projectId)
        {
            string continuationToken = null;
            List<GroupModel> _fromApi = new List<GroupModel>();

            // get descriptor for this project
            string scopeDescriptor = await GetDescriptorAsync(projectId);

            while (true)
            {
                var _uri = _uriHelper.Get(Methods.Groups,
                new UriParameters()
                {
                    scopeDescriptor = scopeDescriptor,
                });

                HttpClientHelper request = new HttpClientHelper(_credentials, _uri);

                var resp = await request.GetAsync();
                continuationToken = resp.ContinuationToken;
                string json = resp.Content;

                GroupReference.Root groups = JsonConvert.DeserializeObject<GroupReference.Root>(json);

                var _groups = groups.value.Select(p => new GroupModel()
                {
                    Id = 0,
                    IsActive = true,
                    LoadDate = DateTime.Now,
                    Descriptor = p.description,
                    DisplayName = p.displayName,
                    Domain = p.domain,
                    MailAddress = p.mailAddress,
                    Origin = p.origin,
                    OriginId = p.originId,
                    PrincipalName = p.principalName,
                    ProjectId = projectId,
                    Url = p.url,
                    
                }).ToList();

                _fromApi.AddRange(_groups);

                if (string.IsNullOrEmpty(continuationToken))
                    break;

            }
                        
            return _fromApi;

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
        // ~Graph()
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
