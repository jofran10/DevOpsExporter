using DataTransfer.Areas;
using DataTransfer.Enums;
using DataTransfer.Helper;
using System;
using System.Threading.Tasks;

namespace DataTransfer.Service
{
    public class DataTransferService: IDataTransferService, IDisposable
    {
        private bool disposedValue;

        public async Task<int> LoadAllData()
        {
            int ret = 0;

            ret = await LoadProjectsAsync();
            ret = ret + await LoadPipelinesAsync();
            ret = ret + await LoadRepositoriesAsync();
            ret = ret + await LoadGroupsAsync();
            ret = ret + await LoadCommitsAsync();
            //ret = ret + await LoadPipelinesRunsAsync();
            ret = ret + await LoadBuildsAsync();
            ret = ret + await LoadReleaseDefinitionsAsync();
            ret = ret + await LoadReleasesAsync();
            ret = ret + await LoadReleaseApprovalsAsync();
            ret = ret + await CheckPendingApprovalsAsync();


            return ret;
        }

        private async Task<int> LoadProjectsAsync()
        {
            try
            {
                var _projects = new Projects();
                return await _projects.ListAllAsync();
               
            }
            catch (Exception ex)
            {
                await SaveLoadHistory(Methods.Projects, -1, false, ex.Message);
                throw ex;
            }
            

        }

        private async Task<int> LoadPipelinesAsync()
        {
            try
            {
                var _pipelines = new Pipelines();
                return await _pipelines.ListAllPipelinesAsync();

            }
            catch (Exception ex)
            {
                await SaveLoadHistory(Methods.Pipelines, -1, false, ex.Message);
                throw ex;
            }


        }

        private async Task<int> LoadGroupsAsync()
        {
            try
            {
                var _graph = new Graph();
                return await _graph.ListAllGroupsAsync();

            }
            catch (Exception ex)
            {
                await SaveLoadHistory(Methods.Groups, -1, false, ex.Message);
                throw ex;
            }


        }

        private async Task<int> LoadPipelinesRunsAsync()
        {
            try
            {
                var _pipelines = new Pipelines();
                return await _pipelines.ListAllPipelineRunsAsync();

            }
            catch (Exception ex)
            {
                await SaveLoadHistory(Methods.PipelineRuns, -1, false, ex.Message);
                throw ex;
            }


        }

        private async Task<int> LoadRepositoriesAsync()
        {
            try
            {
                var _git = new Git();
                return await _git.ListAllRepositoriesAsync();
               
            }
            catch (Exception ex)
            {
                await SaveLoadHistory(Methods.Repositories, -1, false, ex.Message);
                throw ex;
            }

            
        }

        private async Task<int> LoadCommitsAsync()
        {
            try
            {
                var _git = new Git();
                return await _git.ListAllCommitsAsync();
               
            }
            catch (Exception ex)
            {
                await SaveLoadHistory(Methods.Commits, -1, false, ex.Message);
                throw ex;
            }
                       

        }

        private async Task<int> LoadReleaseDefinitionsAsync()
        {
            try
            {
                var _releases = new Releases();
                return await _releases.ListAllDefinitionsAsync();

            }
            catch (Exception ex)
            {
                await SaveLoadHistory(Methods.ReleaseDefinitions, -1, false, ex.Message);
                throw ex;
            }


        }

        private async Task<int> LoadReleaseApprovalsAsync()
        {
            try
            {
                var _releases = new Releases();
                return await _releases.ListAllApprovalsAsync();

            }
            catch (Exception ex)
            {
                await SaveLoadHistory(Methods.ReleaseApprovals, -1, false, ex.Message);
                throw ex;
            }


        }

        private async Task<int> LoadReleasesAsync()
        {
            try
            {
                var _releases = new Releases();
                return await _releases.ListAllAsync();

            }
            catch (Exception ex)
            {
                await SaveLoadHistory(Methods.Releases, -1, false, ex.Message);
                throw ex;
            }


        }

        private async Task<int> CheckPendingApprovalsAsync()
        {
            try
            {
                var _releases = new Releases();
                return await _releases.CheckPendingApprovalsAsync();

            }
            catch (Exception ex)
            {
                await SaveLoadHistory(Methods.ReleasePendingApprovals, -1, false, ex.Message);
                throw ex;
            }


        }

        private async Task<int> LoadBuildsAsync()
        {
            try
            {
                var _builds = new Builds();
                return await _builds.ListAllAsync();

            }
            catch (Exception ex)
            {
                await SaveLoadHistory(Methods.Builds, -1, false, ex.Message);
                throw ex;
            }


        }

        internal async Task SaveLoadHistory(Methods method, int result, bool isSuccessful, string message)
        {
            
            var _utils = new Utils();
            await _utils.SaveLoadHistory(method, result, isSuccessful, message);
            
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
        
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
