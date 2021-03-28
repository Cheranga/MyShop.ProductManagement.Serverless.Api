using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace MyShop.ProductManagement.Serverless.Api.Functions.V2
{
    public class UpdateProductFunction
    {
        [FunctionName(nameof(UpdateProductFunction))]
        public async Task Run(
            [ServiceBusTrigger("%ServiceBusConfig:WriteTopic%", "%ServiceBusConfig:UpdateProductSubscription%", Connection = "ServiceBusConfig:ReadConnectionString", IsSessionsEnabled = true)]
            Message message,
            IMessageSession messageSession, string lockToken)
        {
            await messageSession.CompleteAsync(lockToken);

        }
    }
}
