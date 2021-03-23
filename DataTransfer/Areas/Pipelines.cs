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
    public class Pipelines : IDisposable
    {

        private Utils _utils;
        private UriHelper _uriHelper;
        private string _credentials;
        private bool disposedValue;

        public Pipelines()
        {
            _utils = new Utils();
            _uriHelper = new UriHelper();
            _credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", Environment.GetEnvironmentVariable("PersonalAccessToken"))));

        }

        public async Task<int> ListAllPipelinesAsync()
        {
            int ret = 0;
            string restoreKey = "TbProjects.Id";

            if (_utils.AlreadyBeenExecuted(Methods.Pipelines))
            {
                await _utils.SaveLoadHistory(Methods.Pipelines, ret, true, "load has already been successfully executed");
                return 0;
            }

            // all projs
            string aux;
            int intPId = 0;

            var restorePoint = _utils.GetRestorePoint(Methods.Pipelines, DateTime.Today);
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

                var pipelines = await GetAllPipelinesByProjectId(proj.ProjectId);
                if (pipelines.count <= 0)
                    continue;

                List<PipelineModel> _fromApi = new List<PipelineModel>();

                foreach (var p in pipelines.value)
                {
                    _fromApi.Add(new PipelineModel()
                    {
                        Folder = p.folder,
                        PipelineId = p.id,
                        Revision = p.revision,
                        SelfLink = p._links.self.href,
                        WebLink = p._links.web.href,
                        IsActive = true,
                        LoadDate = DateTime.Now,
                        Name = p.name,
                        ProjectId = proj.ProjectId,
                        Url = p.url,

                    });
                }

                using (DbDevOpsDashContext ctx = new DbDevOpsDashContext())
                {
                    var _inDb = await ctx.Pipelines
                            .Where(p => p.ProjectId == proj.ProjectId && p.IsActive == true)
                            .AsNoTracking()
                            .ToListAsync();

                    // get ID from Db
                    foreach (var inDb in _inDb)
                    {
                        if (_fromApi.Where(fromApi => fromApi.ProjectId == inDb.ProjectId && fromApi.PipelineId == inDb.PipelineId).Any())
                            _fromApi.Where(fromApi => fromApi.ProjectId == inDb.ProjectId && fromApi.PipelineId == inDb.PipelineId)
                                .FirstOrDefault().Id = inDb.Id;
                    }

                    // to update
                    var _toUpdate = _fromApi
                        .Intersect(_inDb, new PipelineEqualityComparer())
                        .Select(j => new PipelineModel
                        {
                            Id = j.Id,
                            Name = j.Name,
                            Url = j.Url,
                            LoadDate = DateTime.Now,
                            IsActive = j.IsActive,
                            Folder = j.Folder,
                            ProjectId = j.ProjectId,
                            Revision = j.Revision,
                            SelfLink = j.SelfLink,
                            WebLink = j.WebLink,
                            PipelineId = j.PipelineId
                        });

                    ret = ret + _toUpdate.Count();
                    ctx.Pipelines.UpdateRange(_toUpdate);

                    // to delete
                    var _toDelete = _inDb.Where(inDb => !_fromApi.Any(fromApi => fromApi.ProjectId == inDb.ProjectId && fromApi.PipelineId == inDb.PipelineId)).ToList();
                    _toDelete.ToList().ForEach(x => { x.IsActive = false; x.LoadDate = DateTime.Now; });
                    ret = ret + _toDelete.Count;
                    ctx.Pipelines.UpdateRange(_toDelete);

                    // to add
                    var _toAdd = _fromApi.Where(fromApi => !_inDb.Any(inDb => fromApi.ProjectId == inDb.ProjectId && fromApi.PipelineId == inDb.PipelineId)).ToList();
                    await ctx.Pipelines.AddRangeAsync(_toAdd);
                    ret = ret + _toAdd.Count;

                    // restore point
                    var rpm = new LoadRestoreModel()
                    {
                        ExecutionDateTime = DateTime.Now,
                        LastId = $"{restoreKey}={proj.Id}",
                        LoadDate = DateTime.Today,
                        Method = (int)Methods.Pipelines,
                        Id = (int)Methods.Pipelines,
                    };

                    ctx.LoadRestorePoint.Update(rpm);

                    // commit
                    await ctx.SaveChangesAsync();

                }

            }

            await _utils.SaveLoadHistory(Methods.Pipelines, ret, true, "");
            return ret;

        }


        public async Task<int> ListAllPipelineRunsAsync()
        {
            

            if (_utils.AlreadyBeenExecuted(Methods.PipelineRuns))
            {
                await _utils.SaveLoadHistory(Methods.PipelineRuns, 0, true, "load has already been successfully executed");
                return 0;
            }

            int ret = 0;
            string restoreKey = "TbPipelines.Id";

            // all 
            string aux;
            int intPId = 0;

            var restorePoint = _utils.GetRestorePoint(Methods.PipelineRuns, DateTime.Today);
            if (restorePoint.Count > 0)
            {
                restorePoint.TryGetValue(restoreKey, out aux);
                intPId = Convert.ToInt32(aux);
            }


            // all pipelines runs
            DbDevOpsDashContext pipesCtx = new DbDevOpsDashContext();
            var pipes = await pipesCtx.Pipelines
                .AsNoTracking()
                .Where(p => p.Id > intPId && p.IsActive == true).OrderBy(p => p.Id)
                .ToArrayAsync();
            pipesCtx.Dispose();

            foreach (var pipe in pipes)
            {
                List<PipelineRunModel> _fromApi = new List<PipelineRunModel>();

                var pipelines = await GetAllPipelinesRunsByPipelineId(pipe.ProjectId, pipe.PipelineId);

                _fromApi = pipelines.value.Select(p => new PipelineRunModel()
                {
                    CreatedDate = p.createdDate,
                    Name = p.name,
                    FinishedDate = p.finishedDate,
                    Result = p.result,
                    SelfLink = p._links.self.href,
                    State = p.state,
                    Url = p.url,
                    WebLink = p._links.web.href,
                    PipelineId = pipe.PipelineId,
                    PipelineRunId = p.id,
                    IsActive = true,
                    LoadDate = DateTime.Now,
                    ProjectId = pipe.ProjectId,

                }).ToList();

                using (DbDevOpsDashContext ctx = new DbDevOpsDashContext())
                {
                    // verificar alterações/inclusões/exclusões
                    var _inDb = await ctx.PipelineRuns
                        .Where(inDb => inDb.PipelineId == pipe.PipelineId && inDb.IsActive == true)
                        .AsNoTracking()
                        .ToListAsync();

                    // get ID from Db
                    foreach (var inDb in _inDb)
                    {
                        if (_fromApi.Where(fromApi => fromApi.PipelineRunId == inDb.PipelineId).Any())
                            _fromApi.Where(fromApi => fromApi.PipelineRunId == inDb.PipelineRunId).FirstOrDefault().Id = inDb.Id;
                    }

                    // projs to update
                    var _toUpdate = _fromApi
                        .Intersect(_inDb, new PipelineRunEqualityComparer())
                        .Select(j => new PipelineRunModel
                        {
                            Id = j.Id,
                            Name = j.Name,
                            Url = j.Url,
                            LoadDate = DateTime.Now,
                            IsActive = j.IsActive,
                            SelfLink = j.SelfLink,
                            WebLink = j.WebLink,
                            CreatedDate = j.CreatedDate,
                            FinishedDate = j.FinishedDate,
                            PipelineId = j.PipelineId,
                            PipelineRunId = j.PipelineRunId,
                            Result = j.Result,
                            State = j.State,
                            ProjectId = j.ProjectId,

                        });

                    ret = ret + _toUpdate.Count();
                    ctx.UpdateRange(_toUpdate);

                    // to delete
                    var _toDelete = _inDb
                        .Where(inDb => !_fromApi.Any(fromApi => fromApi.PipelineId == inDb.PipelineId && fromApi.PipelineRunId == inDb.PipelineRunId)).ToList();
                    _toDelete.ToList().ForEach(x => { x.IsActive = false; x.LoadDate = DateTime.Now; });
                    ret = ret + _toDelete.Count;
                    ctx.UpdateRange(_toDelete);

                    // projs to add
                    var _toAdd = _fromApi
                        .Where(fromApi => !_inDb.Any(inDb => inDb.PipelineId == fromApi.PipelineId && inDb.PipelineRunId == fromApi.PipelineRunId)).ToList();
                    await ctx.AddRangeAsync(_toAdd);
                    ret = ret + _toAdd.Count;

                    // restore point
                    var rpm = new LoadRestoreModel()
                    {
                        ExecutionDateTime = DateTime.Now,
                        LastId = $"{restoreKey}={pipe.Id}",
                        LoadDate = DateTime.Today,
                        Method = (int)Methods.PipelineRuns,
                        Id = (int)Methods.PipelineRuns,
                    };

                    ctx.LoadRestorePoint.Update(rpm);

                    await ctx.SaveChangesAsync();

                }
            }

            await _utils.SaveLoadHistory(Methods.PipelineRuns, ret, true, "");
            return ret;

        }



        private async Task<PipelineReference.Root> GetAllPipelinesByProjectId(string projectId)
        {
            var _uri = _uriHelper.Get(Methods.Pipelines,
                new UriParameters()
                {
                    project = projectId
                });

            HttpClientHelper request = new HttpClientHelper(_credentials, _uri);
            var resp = await request.GetAsync();
            string json = resp.Content;

            PipelineReference.Root pipelines = JsonConvert.DeserializeObject<PipelineReference.Root>(json);
            return pipelines;

        }


        private async Task<PipelineRunReference.Root> GetAllPipelinesRunsByPipelineId(string projectId, int pipelineId)
        {
            var _uri = _uriHelper.Get(Methods.PipelineRuns,
                new UriParameters()
                {
                    project = projectId,
                    pipelineId = pipelineId.ToString()
                });
                        
            HttpClientHelper request = new HttpClientHelper(_credentials, _uri);
            var resp = await request.GetAsync();
            string json = resp.Content;

            PipelineRunReference.Root runs = JsonConvert.DeserializeObject<PipelineRunReference.Root>(json);
            return runs;

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
        // ~Pipelines()
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
