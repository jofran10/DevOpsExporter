using System;
using System.Threading.Tasks;
using DataTransfer.Service;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace TimerTrigger
{
    public class Worker
    {
        private readonly IDataTransferService _service;

        public Worker(IDataTransferService service)
        {
            
            this._service = service;
        }
                

        [FunctionName("DevOpsDashDataTransfer")]
        //public async Task RunAsync([TimerTrigger("0 0 11-16 * * *")]TimerInfo myTimer, ILogger log)
        public async Task RunAsync([TimerTrigger("0 * * * * *")] TimerInfo myTimer, ILogger log)
        {
            try
            {
                var result = await _service.LoadAllData();
                log.LogInformation($"DevOpsDashDataTransfer Timer trigger function executed at: {DateTime.Now}, {result}");
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
            }
        }
    }
}
