using DataTransfer.Context;
using DataTransfer.Enums;
using DataTransfer.Extensions.Enums;
using DataTransfer.Helper;
using DataTransfer.Model.Api;
using DataTransfer.Model.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32.SafeHandles;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer.Services
{
    public class DevOpsService : IDisposable
    {
        private string _credentials;
        private string _baseUrl;
        private string _orgName;
        private string _apisEndPoint;

        private string _area;
        private string _resource;
        private string _resourceId;
        private string _subArea;
        private string _apiVersion;

        public DevOpsService()
        {
            _credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", Environment.GetEnvironmentVariable("PersonalAccessToken"))));
            _baseUrl = Environment.GetEnvironmentVariable("BaseUrl");
            _orgName = Environment.GetEnvironmentVariable("Organization");
            _apisEndPoint = Environment.GetEnvironmentVariable("ApisEndPoint");
        }














        
        



        

        #region Disposed https://docs.microsoft.com/pt-br/dotnet/standard/garbage-collection/implementing-dispose
        // Flag: Has Dispose already been called?
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                // Free any other managed objects here.
                //
            }

            disposed = true;
        }

        #endregion
    }

}
