using DataTransfer.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataTransfer.Helper
{
    public class UriHelper
    {
        public Uri Get(Methods method, UriParameters parms)
        {
            string organization = Environment.GetEnvironmentVariable("Organization");

            string fromDate = parms.fromDate;
            string pipelineId = parms.pipelineId;
            string project = parms.project;
            string repositoryId = parms.repositoryId;
            string toDate = parms.toDate;
            string definitionId = parms.definitionId;
            string continuationToken = parms.continuationToken;
            string subjectDescriptor = parms.subjectDescriptor;
            string storageKey = parms.storageKey;
            string scopeDescriptor = parms.scopeDescriptor;
            string assignedToFilter = parms.assignedToFilter;
            string statusFilter = parms.statusFilter;
            string typeFilter = parms.typeFilter;
            string releaseIdsFilter = parms.releaseIdsFilter;
            string queryOrder = parms.queryOrder;

            string _uri = "";

            switch (method)
            {
                case Methods.Projects:
                    _uri = $"https://dev.azure.com/{organization}/_apis/projects?api-version=6.1-preview.4";
                    break;

                case Methods.Builds:
                    _uri = $"https://dev.azure.com/{organization}/{project}/_apis/build/builds?api-version=6.1-preview.6" +
                           $"&minTime={fromDate}" + 
                           $"&maxTime={toDate}";
                    break;

                case Methods.Commits:
                    _uri = $"https://dev.azure.com/{organization}/_apis/git/repositories/{repositoryId}/commits?api-version=6.1-preview.1" +
                           $"&searchCriteria.fromDate={fromDate}" +
                           $"&searchCriteria.toDate={toDate}";
                    break;

                case Methods.Repositories:
                    _uri = $"https://dev.azure.com/{organization}/_apis/git/repositories?api-version=6.1-preview.1";
                    break;

                case Methods.Pipelines:
                    _uri = $"https://dev.azure.com/{organization}/{project}/_apis/pipelines?api-version=6.1-preview.1";
                    break;

                case Methods.PipelineRuns:
                    _uri = $"https://dev.azure.com/{organization}/{project}/_apis/pipelines/{pipelineId}/runs?api-version=6.1-preview.1";
                    break;

                case Methods.ReleaseDefinitions:
                    _uri = $"https://vsrm.dev.azure.com/{organization}/{project}/_apis/release/definitions?api-version=6.1-preview.4";
                    break;

                case Methods.ReleaseApprovals:
                    _uri = $"https://vsrm.dev.azure.com/{organization}/{project}/_apis/release/approvals?api-version=6.1-preview.3";

                    if(!string.IsNullOrEmpty(assignedToFilter))
                        _uri += $"&assignedToFilter={assignedToFilter}";

                    if (!string.IsNullOrEmpty(statusFilter))
                        _uri += $"&statusFilter={statusFilter}";
                    
                    if (!string.IsNullOrEmpty(typeFilter))
                        _uri += $"&typeFilter={typeFilter}";

                    if (!string.IsNullOrEmpty(queryOrder))
                        _uri += $"&queryOrder={queryOrder}";

                    if (!string.IsNullOrEmpty(continuationToken))
                        _uri += $"&continuationToken={continuationToken}";

                    break;

                case Methods.ReleasePendingApprovals:
                    _uri = $"https://vsrm.dev.azure.com/{organization}/{project}/_apis/release/approvals?api-version=6.1-preview.3" +
                           $"&assignedToFilter={assignedToFilter}" + 
                           $"&statusFilter={statusFilter}" + 
                           $"&releaseIdsFilter={releaseIdsFilter}";
                    break;

                case Methods.Releases:
                    _uri = $"https://vsrm.dev.azure.com/{organization}/{project}/_apis/release/releases?api-version=6.1-preview.8" +
                           $"&minCreatedTime={fromDate}" +
                           $"&maxCreatedTime={toDate}";

                    if (!string.IsNullOrEmpty(continuationToken))
                        _uri += $"&continuationToken={continuationToken}";

                    break;

                case Methods.Descriptor:
                    _uri = $"https://vssps.dev.azure.com/{organization}/_apis/graph/descriptors/{storageKey}?api-version=6.1-preview.1";
                    break;

                case Methods.StorageKey:
                    _uri = $"https://vssps.dev.azure.com/{organization}/_apis/graph/storagekeys/{subjectDescriptor}?api-version=6.1-preview.1";
                    break;

                case Methods.Groups:
                    _uri = $"https://vssps.dev.azure.com/{organization}/_apis/graph/groups?api-version=6.1-preview.1";

                    if (!string.IsNullOrEmpty(scopeDescriptor))
                        _uri += $"&scopeDescriptor={scopeDescriptor}";

                    if (!string.IsNullOrEmpty(continuationToken))
                        _uri += $"&continuationToken={continuationToken}";
                    break;

                default:
                    break;

            }

            return new Uri(_uri);
        }
    }

    public class UriParameters
    {
        
        public string project { get; set; }
        public string repositoryId { get; set; }
        public string pipelineId { get; set; }
        public string definitionId { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public string continuationToken { get; set; }
        public string storageKey { get; set; }
        public string subjectDescriptor { get; set; }
        public string scopeDescriptor { get; set; }
        public string assignedToFilter { get; set; }
        public string statusFilter { get; set; } 
        public string typeFilter { get; set; } 
        public string releaseIdsFilter { get; set; }
        public string queryOrder { get; set; }

    }



}
