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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer.Areas
{
    public class Releases : IDisposable
    {
        private Utils _utils;
        private UriHelper _uriHelper;
        private string _credentials;
        private bool disposedValue;

        public Releases()
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
            int daysBack = Convert.ToInt32(Environment.GetEnvironmentVariable("BackDaysForInspect").ToString()) * -1;

            //searchCriteria.fromDate - If provided, only include history entries created after this date(string)
            DateTime fromDate = _utils.GetLastToDate(Methods.Releases);

            // se fromDate = minvalue -> executar carga inicial a partir de 01/01/2020 até data de hoje
            if (fromDate >= DateTime.Today)
            {
                await _utils.SaveLoadHistory(Methods.Releases, ret, true, "load has already been successfully executed");
                return 0;
            }

            // list
            DbDevOpsDashContext projsCtx = new DbDevOpsDashContext();
            var projs = await projsCtx.Projects
                .Where(p => p.IsActive == true)
                .OrderBy(p => p.Id)
                .AsNoTracking().ToListAsync();
            await projsCtx.DisposeAsync();

            //searchCriteria.fromDate - If provided, only include history entries created after this date(string)
            if (fromDate < minFromDate)
                fromDate = minFromDate;

            //searchCriteria.toDate   - If provided, only include history entries created before this date(string)
            DateTime toDate = fromDate.AddDays(dateIncrement);

            if (toDate > maxToDate)
                toDate = maxToDate;

            while (true)
            {
                List<ReleaseModel> _fromApi = new List<ReleaseModel>();

                using (DbDevOpsDashContext ctx = new DbDevOpsDashContext())
                {
                    try
                    {
                        foreach (var p in projs)
                        {
                            // days back
                            DateTime fromDateBack = fromDate.AddDays(daysBack);
                            var _tmpApi = await GetAllReleasesByProjectId(p.ProjectId, fromDateBack, toDate);

                            _fromApi.AddRange(_tmpApi);

                        }

                        var _inDb = await ctx.Releases.Where(p => p.IsActive == true).AsNoTracking().ToListAsync();

                        // get ID from Db
                        foreach (var inDb in _inDb)
                        {
                            if (_fromApi.Where(fromApi => fromApi.ProjectId == inDb.ProjectId && fromApi.ReleaseId == inDb.ReleaseId).Any())
                                _fromApi.Where(fromApi => fromApi.ProjectId == inDb.ProjectId && fromApi.ReleaseId == inDb.ReleaseId).FirstOrDefault().Id = inDb.Id;
                        }

                        // to update
                        var _toUpdate = _fromApi
                            .Intersect(_inDb, new ReleaseEqualityComparer())
                            .Select(j => new ReleaseModel
                            {
                                CreatedBy = j.CreatedBy,
                                CreatedByDescriptor = j.CreatedByDescriptor,
                                CreatedById = j.CreatedById,
                                CreatedByImageUrl = j.CreatedByImageUrl,
                                CreatedByUniqueName = j.CreatedByUniqueName,
                                CreatedByUrl = j.CreatedByUrl,
                                CreatedFor = j.CreatedFor,
                                ReleaseId = j.ReleaseId,
                                CreatedForDescriptor = j.CreatedForDescriptor,
                                CreatedForId = j.CreatedForId,
                                CreatedForImageUrl = j.CreatedForImageUrl,
                                CreatedForUniqueName = j.CreatedForUniqueName,
                                CreatedForUrl = j.CreatedForUrl,
                                CreatedOn = j.CreatedOn,
                                DefinitionId = j.DefinitionId,
                                DefinitionName = j.DefinitionName,
                                DefinitionPath = j.DefinitionPath,
                                DefinitionSelfLink = j.DefinitionSelfLink,
                                DefinitionSnapshotRevision = j.DefinitionSnapshotRevision,
                                DefinitionUrl = j.DefinitionUrl,
                                DefinitionWebLink = j.DefinitionWebLink,
                                Description = j.Description,
                                Id = j.Id,
                                IsActive = j.IsActive,
                                KeepForever = j.KeepForever,
                                LoadDate = DateTime.Now,
                                LogsContainerUrl = j.LogsContainerUrl,
                                ModifiedBy = j.ModifiedBy,
                                ModifiedByDescriptor = j.ModifiedByDescriptor,
                                ModifiedById = j.ModifiedById,
                                ModifiedByImageUrl = j.ModifiedByImageUrl,
                                ModifiedByUniqueName = j.ModifiedByUniqueName,
                                ModifiedByUrl = j.ModifiedByUrl,
                                ModifiedOn = j.ModifiedOn,
                                Name = j.Name,
                                ProjectId = j.ProjectId,
                                ProjectName = j.ProjectName,
                                Reason = j.Reason,
                                ReleaseDefinitionRevision = j.ReleaseDefinitionRevision,
                                ReleaseNameFormat = j.ReleaseNameFormat,
                                SelfLink = j.SelfLink,
                                Status = j.Status,
                                Tags = j.Tags,
                                TriggeringArtifactAlias = j.TriggeringArtifactAlias,
                                Url = j.Url,
                                VariableGroups = j.VariableGroups,
                                Variables = j.Variables,
                                WebLink = j.WebLink,



                            });

                        ret = _toUpdate.Count();
                        ctx.Releases.UpdateRange(_toUpdate);

                        //  to delete
                        //var _toDelete = _inDb.Where(x => !_fromApi.Any(y => y.ProjectId == x.ProjectId && y.ReleaseId == x.ReleaseId)).ToList();
                        //_toDelete.ToList().ForEach(x => { x.IsActive = false; x.LoadDate = DateTime.Now; });
                        //ret = ret + _toDelete.Count;
                        //ctx.Releases.UpdateRange(_toDelete);

                        //  to add
                        //var _toAdd = _fromApi.Where(x => !_inDb.Any(y => y.ProjectId == x.ProjectId && y.ReleaseId == x.ReleaseId)).ToList();
                        var _toAdd = _fromApi.Where(x => x.Id == 0).ToList();
                        await ctx.Releases.AddRangeAsync(_toAdd);
                        ret = ret + _toAdd.Count;

                        // history
                        await ctx.LoadHistory.AddAsync(_utils.LoadHistoryHelper(Methods.Releases, ret, true, "", fromDate, toDate));

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

        public async Task<int> ListAllDefinitionsAsync()
        {
            int ret = 0;
            string restoreKey = "TbReleaseDefinitions.Id";

            if (_utils.AlreadyBeenExecuted(Methods.ReleaseDefinitions))
            {
                await _utils.SaveLoadHistory(Methods.ReleaseDefinitions, ret, true, "load has already been successfully executed");
                return 0;
            }

            // all projs
            string aux;
            int intPId = 0;

            var restorePoint = _utils.GetRestorePoint(Methods.ReleaseDefinitions, DateTime.Today);
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

                var releases = await GetAllReleaseDefinitionsByProjectId(proj.ProjectId);
                if (releases.count <= 0)
                    continue;

                List<ReleaseDefinitionModel> _fromApi = new List<ReleaseDefinitionModel>();

                foreach (var r in releases.value)
                {
                    _fromApi.Add(new ReleaseDefinitionModel()
                    {
                        CreatedBy = r.createdBy.displayName,
                        CreatedByDescriptor = r.createdBy.descriptor,
                        CreatedById = r.createdBy.id,
                        CreatedByImageUrl = r.createdBy.imageUrl,
                        CreatedByUniqueName = r.createdBy.uniqueName,
                        CreatedByUrl = r.createdBy.url,
                        CreatedOn = r.createdOn,
                        DefinitionId = r.id,
                        Description = r.description,
                        IsActive = true,
                        IsDeleted = r.isDeleted,
                        LoadDate = DateTime.Now,
                        ModifiedBy = r.modifiedBy.displayName,
                        ModifiedByDescriptor = r.modifiedBy.descriptor,
                        ModifiedById = r.modifiedBy.id,
                        ModifiedByImageUrl = r.modifiedBy.imageUrl,
                        ModifiedByUniqueName = r.modifiedBy.uniqueName,
                        ModifiedByUrl = r.modifiedBy.url,
                        ModifiedOn = r.modifiedOn,
                        Name = r.name,
                        Path = r.path,
                        ProjectId = proj.ProjectId,
                        ReleaseNameFormat = r.releaseNameFormat,
                        Revision = r.revision,
                        SelfLink = r._links.self.href,
                        Source = r.source,
                        Url = r.url,
                        WebLink = r._links.web.href,
                        Id = 0,

                    });
                }

                using (DbDevOpsDashContext ctx = new DbDevOpsDashContext())
                {
                    var _inDb = await ctx.ReleaseDefinitions
                            .Where(p => p.ProjectId == proj.ProjectId && p.IsActive == true)
                            .AsNoTracking()
                            .ToListAsync();

                    // get ID from Db
                    foreach (var inDb in _inDb)
                    {
                        if (_fromApi.Where(fromApi => fromApi.ProjectId == inDb.ProjectId && fromApi.DefinitionId == inDb.DefinitionId).Any())
                            _fromApi.Where(fromApi => fromApi.ProjectId == inDb.ProjectId && fromApi.DefinitionId == inDb.DefinitionId)
                                .FirstOrDefault().Id = inDb.Id;
                    }

                    // to update
                    var _toUpdate = _fromApi
                        .Intersect(_inDb, new ReleaseDefinitionEqualityComparer())
                        .Select(r => new ReleaseDefinitionModel
                        {
                            CreatedBy = r.CreatedBy,
                            CreatedByDescriptor = r.CreatedByDescriptor,
                            CreatedById = r.CreatedById,
                            CreatedByImageUrl = r.CreatedByImageUrl,
                            CreatedByUniqueName = r.CreatedByUniqueName,
                            CreatedByUrl = r.CreatedByUrl,
                            CreatedOn = r.CreatedOn,
                            DefinitionId = r.DefinitionId,
                            Description = r.Description,
                            IsActive = r.IsActive,
                            IsDeleted = r.IsDeleted,
                            LoadDate = DateTime.Now,
                            ModifiedBy = r.ModifiedBy,
                            ModifiedByDescriptor = r.ModifiedByDescriptor,
                            ModifiedById = r.ModifiedById,
                            ModifiedByImageUrl = r.ModifiedByImageUrl,
                            ModifiedByUniqueName = r.ModifiedByUniqueName,
                            ModifiedByUrl = r.ModifiedByUrl,
                            ModifiedOn = r.ModifiedOn,
                            Name = r.Name,
                            Path = r.Path,
                            ProjectId = proj.ProjectId,
                            ReleaseNameFormat = r.ReleaseNameFormat,
                            Revision = r.Revision,
                            SelfLink = r.SelfLink,
                            Source = r.Source,
                            Url = r.Url,
                            WebLink = r.WebLink,
                            Id = r.Id,

                        });

                    ret = ret + _toUpdate.Count();
                    ctx.ReleaseDefinitions.UpdateRange(_toUpdate);

                    // to delete
                    var _toDelete = _inDb.Where(inDb => !_fromApi.Any(fromApi => fromApi.ProjectId == inDb.ProjectId && fromApi.DefinitionId == inDb.DefinitionId)).ToList();
                    _toDelete.ToList().ForEach(x => { x.IsActive = false; x.LoadDate = DateTime.Now; });
                    ret = ret + _toDelete.Count;
                    ctx.ReleaseDefinitions.UpdateRange(_toDelete);

                    // to add
                    var _toAdd = _fromApi.Where(fromApi => !_inDb.Any(inDb => fromApi.ProjectId == inDb.ProjectId && fromApi.DefinitionId == inDb.DefinitionId)).ToList();
                    await ctx.ReleaseDefinitions.AddRangeAsync(_toAdd);
                    ret = ret + _toAdd.Count;

                    // restore point
                    var rpm = new LoadRestoreModel()
                    {
                        ExecutionDateTime = DateTime.Now,
                        LastId = $"{restoreKey}={proj.Id}",
                        LoadDate = DateTime.Today,
                        Method = (int)Methods.ReleaseDefinitions,
                        Id = (int)Methods.ReleaseDefinitions,
                    };

                    ctx.LoadRestorePoint.Update(rpm);

                    // commit
                    await ctx.SaveChangesAsync();

                }

            }

            await _utils.SaveLoadHistory(Methods.ReleaseDefinitions, ret, true, "");
            return ret;

        }

        public async Task<int> ListAllApprovalsAsync()
        {
            int ret = 0;

            if (_utils.AlreadyBeenExecuted(Methods.ReleaseApprovals))
            {
                await _utils.SaveLoadHistory(Methods.ReleaseApprovals, ret, true, "load has already been successfully executed");
                return 0;
            }

            // list
            DbDevOpsDashContext groupsCtx = new DbDevOpsDashContext();
            var groups = await groupsCtx.Groups
                .Where(p => p.IsActive == true && p.OurGroupCode == (int)Groups.ApplicationOwners)
                .OrderBy(p => p.Id)
                .AsNoTracking().ToListAsync();
            await groupsCtx.DisposeAsync();

            List<ReleaseApprovalModel> _fromApi = new List<ReleaseApprovalModel>();

            using (DbDevOpsDashContext ctx = new DbDevOpsDashContext())
            {
                try
                {
                    foreach (var g in groups)
                    {
                        var _tmpApi = await GetAllApprovalsByProjectIdAndGroupId(g.ProjectId, g.OriginId);
                        _fromApi.AddRange(_tmpApi);

                    }


                    #region comments
                    //var _inDb = await ctx.ReleaseApprovals.Where(p => p.IsActive == true).AsNoTracking().ToListAsync();

                    //// get ID from Db
                    //foreach (var inDb in _inDb)
                    //{
                    //    if (_fromApi.Where(fromApi => fromApi.ProjectId == inDb.ProjectId &&
                    //                                  fromApi.ReleaseId == inDb.ReleaseId &&
                    //                                  fromApi.ApprovalId == inDb.ApprovalId).Any())
                    //        _fromApi.Where(fromApi => fromApi.ProjectId == inDb.ProjectId &&
                    //                                  fromApi.ReleaseId == inDb.ReleaseId &&
                    //                                  fromApi.ApprovalId == inDb.ApprovalId).FirstOrDefault().Id = inDb.Id;
                    //}

                    //// to update
                    //var _toUpdate = _fromApi
                    //    .Intersect(_inDb, new ReleaseApprovalEqualityComparer())
                    //    .Select(j => new ReleaseApprovalModel
                    //    {
                    //        ReleaseId = j.ReleaseId,
                    //        CreatedOn = j.CreatedOn,
                    //        DefinitionId = j.DefinitionId,
                    //        DefinitionName = j.DefinitionName,
                    //        DefinitionPath = j.DefinitionPath,
                    //        DefinitionUrl = j.DefinitionUrl,
                    //        Id = j.Id,
                    //        IsActive = j.IsActive,
                    //        LoadDate = DateTime.Now,
                    //        ModifiedOn = j.ModifiedOn,
                    //        ProjectId = j.ProjectId,
                    //        ApprovalId = j.ApprovalId,
                    //        Status = j.Status,
                    //        Url = j.Url,
                    //        ApprovedByDescriptor = j.ApprovedByDescriptor,
                    //        ApprovalType = j.ApprovalType,
                    //        ApprovedBy = j.ApprovedBy,
                    //        ApprovedById  = j.ApprovedById,
                    //        ApprovedByImageUrl = j.ApprovedByImageUrl,
                    //        ApprovedByUniqueName = j.ApprovedByUniqueName,
                    //        ApprovedByUrl = j.ApprovedByUrl,
                    //        Approver = j.Approver,
                    //        ApproverDescriptor = j.ApproverDescriptor,
                    //        ApproverId = j.ApproverId,
                    //        ApproverImageUrl = j.ApproverImageUrl,
                    //        ApproverUniqueName = j.ApproverUniqueName,
                    //        ApproverUrl = j.ApproverUrl,
                    //        Attempt = j.Attempt,
                    //        Comments = j.Comments,
                    //        EnvironmentId = j.EnvironmentId,
                    //        EnvironmentName = j.EnvironmentName,
                    //        EnvironmentUrl = j.EnvironmentUrl,
                    //        IsAutomated = j.IsAutomated,
                    //        IsNotificationOn = j.IsNotificationOn,
                    //        Rank = j.Rank,
                    //        ReleaseName = j.ReleaseName,
                    //        ReleaseUrl = j.ReleaseUrl,
                    //        Revision = j.Revision,
                    //        TrialNumber = j.TrialNumber,
                    //    });

                    //ret = _toUpdate.Count();
                    //ctx.ReleaseApprovals.UpdateRange(_toUpdate);

                    //  to delete
                    //var _toDelete = _inDb.Where(x => !_fromApi.Any(y => y.ProjectId == x.ProjectId && y.ReleaseId == x.ReleaseId)).ToList();
                    //_toDelete.ToList().ForEach(x => { x.IsActive = false; x.LoadDate = DateTime.Now; });
                    //ret = ret + _toDelete.Count;
                    //ctx.Releases.UpdateRange(_toDelete);

                    //  to add
                    //var _toAdd = _fromApi.Where(x => !_inDb.Any(y => y.ProjectId == x.ProjectId && y.ReleaseId == x.ReleaseId)).ToList();
                    //var _toAdd = _fromApi.Where(x => x.Id == 0).ToList();
                    #endregion


                    await ctx.ReleaseApprovals.AddRangeAsync(_fromApi);
                    ret = ret + _fromApi.Count;

                    // history
                    await ctx.LoadHistory.AddAsync(_utils.LoadHistoryHelper(Methods.ReleaseApprovals, ret, true, "", null, null));

                    await ctx.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw ex;

                }

            }







            return ret;

        }



        public async Task<int> CheckPendingApprovalsAsync()
        {
            int ret = 0;

            if (_utils.AlreadyBeenExecuted(Methods.ReleasePendingApprovals))
            {
                await _utils.SaveLoadHistory(Methods.ReleasePendingApprovals, ret, true, "load has already been successfully executed");
                return 0;
            }

            // list
            DbDevOpsDashContext approvalsCtx = new DbDevOpsDashContext();
            var _pendingsInDb = await approvalsCtx.ReleaseApprovals
                .Where(p => p.IsActive == true && p.Status.Contains("pending"))
                .OrderBy(p => p.Id)
                .AsNoTracking().ToListAsync();
            await approvalsCtx.DisposeAsync();

            List<ReleaseApprovalModel> _approvalsFromApi = new List<ReleaseApprovalModel>();

            using (DbDevOpsDashContext ctx = new DbDevOpsDashContext())
            {
                try
                {
                    foreach (var a in _pendingsInDb)
                    {
                        var _uri = _uriHelper.Get(Methods.ReleasePendingApprovals,
                           new UriParameters()
                           {
                               project = a.ProjectId,
                               assignedToFilter = a.ApproverId,
                               statusFilter = "undefined",
                               releaseIdsFilter = a.ReleaseId.ToString(),
                           });

                        HttpClientHelper request = new HttpClientHelper(_credentials, _uri);

                        var resp = await request.GetAsync();
                        string json = resp.Content;

                        ReleaseApprovalReference.Root _fromApi = JsonConvert.DeserializeObject<ReleaseApprovalReference.Root>(json);

                        var _approvals = _fromApi.value.Select(p => new ReleaseApprovalModel()
                        {
                            ApprovalId = p.id,
                            ApprovalType = p.approvalType,
                            Approver = p.approver.displayName,
                            ApproverDescriptor = p.approver.descriptor,
                            ApproverId = p.approver.id,
                            ApproverImageUrl = p.approver.imageUrl,
                            ApproverUniqueName = p.approver.uniqueName,
                            ApproverUrl = p.approver.url,
                            Attempt = p.attempt,
                            Comments = p.comments,
                            CreatedOn = p.createdOn,
                            DefinitionId = p.releaseDefinition.id,
                            DefinitionName = p.releaseDefinition.name,
                            DefinitionPath = p.releaseDefinition.path,
                            DefinitionUrl = p.releaseDefinition.url,
                            EnvironmentId = p.releaseEnvironment.id,
                            EnvironmentName = p.releaseEnvironment.name,
                            EnvironmentUrl = p.releaseEnvironment.url,
                            IsAutomated = p.isAutomated,
                            IsNotificationOn = p.isNotificationOn,
                            Rank = p.rank,
                            ReleaseId = p.release.id,
                            ReleaseName = p.release.name,
                            ReleaseUrl = p.release.url,
                            Revision = p.revision,
                            Status = p.status,
                            TrialNumber = p.trialNumber,
                            Url = p.url,

                            ProjectId = a.ProjectId,
                            Id = 0,
                            IsActive = true,
                            LoadDate = DateTime.Now,
                            ModifiedOn = p.modifiedOn,

                            ApprovedBy = p.approvedBy == null ? null : p.approvedBy.displayName,
                            ApprovedByDescriptor = p.approvedBy == null ? null : p.approvedBy.descriptor,
                            ApprovedById = p.approvedBy == null ? null : p.approvedBy.id,
                            ApprovedByImageUrl = p.approvedBy == null ? null : p.approvedBy.imageUrl,
                            ApprovedByUniqueName = p.approvedBy == null ? null : p.approvedBy.uniqueName,
                            ApprovedByUrl = p.approvedBy == null ? null : p.approvedBy.url,

                        });

                        _approvalsFromApi.AddRange(_approvals);

                    }

                    // get ID from Db
                    foreach (var inDb in _pendingsInDb)
                    {
                        if (_approvalsFromApi.Where(fromApi => fromApi.ProjectId == inDb.ProjectId &&
                                                    fromApi.ReleaseId == inDb.ReleaseId &&
                                                    fromApi.ApprovalId == inDb.ApprovalId).Any())
                            _approvalsFromApi.Where(fromApi => fromApi.ProjectId == inDb.ProjectId &&
                                                    fromApi.ReleaseId == inDb.ReleaseId &&
                                                    fromApi.ApprovalId == inDb.ApprovalId).FirstOrDefault().Id = inDb.Id;
                    }

                    // to update
                    var _toUpdate = _approvalsFromApi
                        .Intersect(_pendingsInDb, new ReleaseApprovalEqualityComparer())
                        .Select(j => new ReleaseApprovalModel
                        {
                            ReleaseId = j.ReleaseId,
                            CreatedOn = j.CreatedOn,
                            DefinitionId = j.DefinitionId,
                            DefinitionName = j.DefinitionName,
                            DefinitionPath = j.DefinitionPath,
                            DefinitionUrl = j.DefinitionUrl,
                            Id = j.Id,
                            IsActive = j.IsActive,
                            LoadDate = DateTime.Now,
                            ModifiedOn = j.ModifiedOn,
                            ProjectId = j.ProjectId,
                            ApprovalId = j.ApprovalId,
                            Status = j.Status,
                            Url = j.Url,
                            ApprovedByDescriptor = j.ApprovedByDescriptor,
                            ApprovalType = j.ApprovalType,
                            ApprovedBy = j.ApprovedBy,
                            ApprovedById = j.ApprovedById,
                            ApprovedByImageUrl = j.ApprovedByImageUrl,
                            ApprovedByUniqueName = j.ApprovedByUniqueName,
                            ApprovedByUrl = j.ApprovedByUrl,
                            Approver = j.Approver,
                            ApproverDescriptor = j.ApproverDescriptor,
                            ApproverId = j.ApproverId,
                            ApproverImageUrl = j.ApproverImageUrl,
                            ApproverUniqueName = j.ApproverUniqueName,
                            ApproverUrl = j.ApproverUrl,
                            Attempt = j.Attempt,
                            Comments = j.Comments,
                            EnvironmentId = j.EnvironmentId,
                            EnvironmentName = j.EnvironmentName,
                            EnvironmentUrl = j.EnvironmentUrl,
                            IsAutomated = j.IsAutomated,
                            IsNotificationOn = j.IsNotificationOn,
                            Rank = j.Rank,
                            ReleaseName = j.ReleaseName,
                            ReleaseUrl = j.ReleaseUrl,
                            Revision = j.Revision,
                            TrialNumber = j.TrialNumber,
                        });

                    ret = _toUpdate.Count();
                    ctx.ReleaseApprovals.UpdateRange(_toUpdate);

                    // history
                    await ctx.LoadHistory.AddAsync(_utils.LoadHistoryHelper(Methods.ReleasePendingApprovals, ret, true, "", null, null));

                    await ctx.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw ex;

                }

            }

            return ret;
        }


        private async Task<ReleaseDefinitionReference.Root> GetAllReleaseDefinitionsByProjectId(string projectId)
        {
            var _uri = _uriHelper.Get(Methods.ReleaseDefinitions,
               new UriParameters()
               {
                   project = projectId,
               });

            HttpClientHelper request = new HttpClientHelper(_credentials, _uri);
            var resp = await request.GetAsync();
            string json = resp.Content;

            ReleaseDefinitionReference.Root releases = JsonConvert.DeserializeObject<ReleaseDefinitionReference.Root>(json);
            return releases;

        }


        private async Task<List<ReleaseApprovalModel>> GetAllApprovalsByProjectIdAndGroupId(string projectId, string groupId)
        {
            // get continuation token
            string continuationToken = "0";
            using (DbDevOpsDashContext ctx = new DbDevOpsDashContext())
            {
                if (ctx.ReleaseApprovals.Where(r => r.ProjectId == projectId && r.ApproverId == groupId && r.IsActive == true).Any())
                    continuationToken = ctx.ReleaseApprovals
                        .Where(r => r.ProjectId == projectId && r.ApproverId == groupId && r.IsActive == true)
                        .Max(r => r.ApprovalId + 1).ToString();
            }

            List<ReleaseApprovalModel> _fromApi = new List<ReleaseApprovalModel>();

            while (true)
            {
                var _uri = _uriHelper.Get(Methods.ReleaseApprovals,
                new UriParameters()
                {
                    project = projectId,
                    continuationToken = continuationToken,
                    queryOrder = "ascending",
                    assignedToFilter = groupId,
                    statusFilter = "undefined",

                });

                HttpClientHelper request = new HttpClientHelper(_credentials, _uri);

                var resp = await request.GetAsync();
                continuationToken = resp.ContinuationToken;
                string json = resp.Content;

                ReleaseApprovalReference.Root approvals = JsonConvert.DeserializeObject<ReleaseApprovalReference.Root>(json);

                var _approvals = approvals.value.Select(p => new ReleaseApprovalModel()
                {
                    ApprovalId = p.id,
                    ApprovalType = p.approvalType,
                    Approver = p.approver.displayName,
                    ApproverDescriptor = p.approver.descriptor,
                    ApproverId = p.approver.id,
                    ApproverImageUrl = p.approver.imageUrl,
                    ApproverUniqueName = p.approver.uniqueName,
                    ApproverUrl = p.approver.url,
                    Attempt = p.attempt,
                    Comments = p.comments,
                    CreatedOn = p.createdOn,
                    DefinitionId = p.releaseDefinition.id,
                    DefinitionName = p.releaseDefinition.name,
                    DefinitionPath = p.releaseDefinition.path,
                    DefinitionUrl = p.releaseDefinition.url,
                    EnvironmentId = p.releaseEnvironment.id,
                    EnvironmentName = p.releaseEnvironment.name,
                    EnvironmentUrl = p.releaseEnvironment.url,
                    IsAutomated = p.isAutomated,
                    IsNotificationOn = p.isNotificationOn,
                    Rank = p.rank,
                    ReleaseId = p.release.id,
                    ReleaseName = p.release.name,
                    ReleaseUrl = p.release.url,
                    Revision = p.revision,
                    Status = p.status,
                    TrialNumber = p.trialNumber,
                    Url = p.url,

                    ProjectId = projectId,
                    Id = 0,
                    IsActive = true,
                    LoadDate = DateTime.Now,
                    ModifiedOn = p.modifiedOn,

                    ApprovedBy = p.approvedBy == null ? null : p.approvedBy.displayName,
                    ApprovedByDescriptor = p.approvedBy == null ? null : p.approvedBy.descriptor,
                    ApprovedById = p.approvedBy == null ? null : p.approvedBy.id,
                    ApprovedByImageUrl = p.approvedBy == null ? null : p.approvedBy.imageUrl,
                    ApprovedByUniqueName = p.approvedBy == null ? null : p.approvedBy.uniqueName,
                    ApprovedByUrl = p.approvedBy == null ? null : p.approvedBy.url,






                }).ToList();

                _fromApi.AddRange(_approvals);

                if (string.IsNullOrEmpty(continuationToken))
                    break;

            }

            return _fromApi;


        }


        private async Task<List<ReleaseModel>> GetAllReleasesByProjectId(string projectId, DateTime fromDate, DateTime toDate)
        {
            string continuationToken = null;
            List<ReleaseModel> _fromApi = new List<ReleaseModel>();

            while (true)
            {
                var _uri = _uriHelper.Get(Methods.Releases,
                new UriParameters()
                {
                    project = projectId,
                    continuationToken = continuationToken,
                    fromDate = fromDate.ToString("yyyy-MM-dd"),
                    toDate = toDate.ToString("yyyy-MM-dd"),

                });

                HttpClientHelper request = new HttpClientHelper(_credentials, _uri);

                var resp = await request.GetAsync();
                continuationToken = resp.ContinuationToken;
                string json = resp.Content;

                ReleaseReference.Root releases = JsonConvert.DeserializeObject<ReleaseReference.Root>(json);

                var _releases = releases.value.Select(p => new ReleaseModel()
                {
                    CreatedBy = p.createdBy.displayName,
                    CreatedByDescriptor = p.createdBy.descriptor,
                    CreatedById = p.createdBy.id,
                    CreatedByImageUrl = p.createdBy.imageUrl,
                    CreatedByUniqueName = p.createdBy.uniqueName,
                    CreatedByUrl = p.createdBy.url,
                    CreatedFor = p.createdFor.displayName,
                    CreatedForDescriptor = p.createdFor.descriptor,
                    CreatedForId = p.createdFor.id,
                    CreatedForImageUrl = p.createdFor.imageUrl,
                    CreatedForUniqueName = p.createdFor.uniqueName,
                    CreatedForUrl = p.createdFor.url,
                    CreatedOn = p.createdOn,
                    DefinitionId = p.releaseDefinition.id,
                    DefinitionName = p.releaseDefinition.name,
                    DefinitionPath = p.releaseDefinition.path,
                    DefinitionSelfLink = p.releaseDefinition._links.self.href,
                    DefinitionSnapshotRevision = p.definitionSnapshotRevision,
                    DefinitionUrl = p.releaseDefinition.url,
                    DefinitionWebLink = p.releaseDefinition._links.web.href,
                    Description = p.description,
                    Id = 0,
                    IsActive = true,
                    KeepForever = p.keepForever,
                    LoadDate = DateTime.Now,
                    LogsContainerUrl = p.logsContainerUrl,
                    ModifiedBy = p.modifiedBy.displayName,
                    ModifiedByDescriptor = p.modifiedBy.descriptor,
                    ModifiedById = p.modifiedBy.id,
                    ModifiedByImageUrl = p.modifiedBy.imageUrl,
                    ModifiedByUniqueName = p.modifiedBy.uniqueName,
                    ModifiedByUrl = p.modifiedBy.url,
                    ModifiedOn = p.modifiedOn,
                    Name = p.name,
                    ProjectId = p.projectReference.id,
                    ProjectName = p.projectReference.name,
                    Reason = p.reason,
                    ReleaseDefinitionRevision = p.releaseDefinitionRevision,
                    ReleaseId = p.id,
                    ReleaseNameFormat = p.releaseNameFormat,
                    SelfLink = p._links.self.href,
                    Status = p.status,
                    Tags = "", //p.tags.
                    TriggeringArtifactAlias = "", //p.triggeringArtifactAlias,
                    Url = p.url,
                    VariableGroups = "", //p.variableGroups,
                    Variables = "", //p.variables,
                    WebLink = p._links.web.href,

                }).ToList();

                _fromApi.AddRange(_releases);

                //Console.WriteLine($"projeto: {projectId}, count: {_fromApi.Count}, ini: {fromDate} , fim: {toDate}, cont: {continuationToken}");

                if (string.IsNullOrEmpty(continuationToken))
                    break;

            }

            //Console.WriteLine($"----------------------- count: {_fromApi.Count}, fim-releases ----------------------------------------");
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
        // ~Releases()
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
