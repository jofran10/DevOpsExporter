using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer.Service
{
    public interface IDataTransferService
    {
        Task<int> LoadAllData();
    }
}
