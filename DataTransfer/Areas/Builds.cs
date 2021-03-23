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
    public class Builds: IDisposable
    {
        private Utils _utils;
        private UriHelper _uriHelper;
        private string _credentials;
        private bool disposedValue;

        public Builds()
        {
            _utils = new Utils();
            _uriHelper = new UriHelper();
            _credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", Environment.GetEnvironmentVariable("PersonalAccessToken"))));

        }

        public async Task<int> ListAllAsync()
        {
            int ret = 0;
            DateTime maxToDate = DateTime.Today;
            DateTime minFromDate = Convert.ToDateTime(Environment.GetEnvironmentVariable("MinFromDate").ToString());
            int dateIncrement = Convert.ToInt32(Environment.GetEnvironmentVariable("DateIncrement").ToString());

            //searchCriteria.fromDate - If provided, only include history entries created after this date(string)
            DateTime fromDate = _utils.GetLastToDate(Methods.Builds);

            // se fromDate = minvalue -> executar carga inicial a partir de 01/01/2020 até data de hoje
            if (fromDate >= DateTime.Today)
            {
                await _utils.SaveLoadHistory(Methods.Builds, ret, true, "load has already been successfully executed");
                return 0;
            }

            // list
            DbDevOpsDashContext pipesCtx = new DbDevOpsDashContext();
            var pipes = await pipesCtx.Projects
                .Where(p => p.IsActive == true)
                .OrderBy(p => p.Id)
                .AsNoTracking().ToListAsync();
            await pipesCtx.DisposeAsync();

            //searchCriteria.fromDate - If provided, only include history entries created after this date(string)
            if (fromDate < minFromDate)
                fromDate = minFromDate;

            //searchCriteria.toDate   - If provided, only include history entries created before this date(string)
            DateTime toDate = fromDate.AddDays(dateIncrement);

            if (toDate > maxToDate)
                toDate = maxToDate;

            while (true)
            {
                List<BuildModel> _fromApi = new List<BuildModel>();

                using (DbDevOpsDashContext ctx = new DbDevOpsDashContext())
                {
                    try
                    {
                        foreach (var p in pipes)
                        {
                            var builds = await GetAllBuildsByProjectId(p.ProjectId, fromDate, toDate);

                            var _tmpApi = builds.value.Select(v => new BuildModel()
                            {
                                BuildId = v.id,
                                BuildNumber = v.buildNumber,
                                BuildNumberRevision = v.buildNumberRevision,
                                BuildUri = v.uri,
                                BuildUrl = v.url,
                                //Definition_Drafts = v.definition.drafts == null ? null : v.definition.drafts.ToString(),
                                Definition_Id = v.definition.id,
                                Definition_Name = v.definition.name,
                                Definition_Url = v.definition.url,
                                FinishTime = v.finishTime,
                                Id = 0,
                                IsActive = true,
                                KeepForever = v.keepForever,
                                LastChangeDate = v.lastChangedDate,
                                Links_Badge = v._links.badge.href,
                                Links_Self = v._links.self.href,
                                Links_SourceVersionDisplayUri = v._links.sourceVersionDisplayUri.href,
                                Links_Timeline = v._links.timeline.href,
                                Links_Web = v._links.web.href,
                                LoadDate = DateTime.Today,
                                LogUrl = v.logs.url,
                                OrchestrationPlan = v.orchestrationPlan.planId,
                                //Plans = v.plans == null ? null : v.plans.ToString(),
                                PoolId = v.queue.pool.id.ToString(),
                                Priority = v.priority,
                                ProjectId = v.project.id,
                                ProjectUrl = v.project.url,
                                //Properties = v.properties == null ? null : v.properties.ToString(),
                                QueueId = v.queue.id.ToString(),
                                QueueName = v.queue.name,
                                QueueTime = v.queueTime,
                                Reason = v.reason,
                                RepositoryId = v.repository.id,
                                RepositoryUrl = v.repository.url,
                                RequestedForAadDescriptor = v.requestedFor.descriptor,
                                RequestedForId = v.requestedFor.id,
                                RequestedForIdentitieLink = v.requestedFor.url,
                                RequestedForName = v.requestedFor.displayName,
                                RequestedForUniqueName = v.requestedFor.uniqueName,
                                Result = v.result,
                                RetainedByRelease = v.retainedByRelease,
                                SourceBranch = v.sourceBranch,
                                SourceVersion = v.sourceVersion,
                                StartTime = v.startTime,
                                Status = v.status,
                                //Tags = v.tags == null ? null : v.tags.ToString(),
                                TriggeredByBuild = v.triggeredByBuild == null ? null : v.triggeredByBuild.ToString(),
                                //TriggerInfo = v.triggerInfo == null ? null : v.triggerInfo.ToString(),
                                //ValidationResults = v.validationResults == null ? null : v.validationResults.ToString(),
                            }).ToList();

                            _fromApi.AddRange(_tmpApi);

                        }

                        // add
                        await ctx.Builds.AddRangeAsync(_fromApi);

                        // history
                        await ctx.LoadHistory.AddAsync(_utils.LoadHistoryHelper(Methods.Builds, _fromApi.Count, true, "", fromDate, toDate));

                        // save
                        await ctx.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        throw ex;

                    }

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

        private async Task<BuildReference.Root> GetAllBuildsByProjectId(string projectId, DateTime fromDate, DateTime toDate)
        {
            var _uri = _uriHelper.Get(Methods.Builds,
                new UriParameters()
                {
                    project = projectId,
                    fromDate = fromDate.ToString("yyyy-MM-dd"),
                    toDate = toDate.ToString("yyyy-MM-dd")
                });
                        
            HttpClientHelper request = new HttpClientHelper(_credentials, _uri);
            var resp = await request.GetAsync();
            string json = resp.Content;

            BuildReference.Root builds = JsonConvert.DeserializeObject<BuildReference.Root>(json);
            return builds;

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
        // ~Builds()
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
