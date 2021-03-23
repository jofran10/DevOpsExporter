using System;
using System.Collections.Generic;
using System.Text;
using DataTransfer.Service;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[assembly: FunctionsStartup(typeof(TimerTrigger.Startup))]
namespace TimerTrigger
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<IDataTransferService>((s) => {
                return new DataTransferService();
            });

            
        }
    }
}
