using System;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Azure.WebJobs;

namespace MyShop.ProductManagement.Serverless.Api.Functions.V2
{
    public class CreateProductFunction
    {
        [FunctionName(nameof(CreateProductFunction))]
        public async Task Run([ServiceBusTrigger("%ServiceBusConfig:WriteTopic%", "%ServiceBusConfig:CreateProductSubscription%", Connection = "ServiceBusConfig:ReadConnectionString")] Message message,
            MessageReceiver messageReceiver, string lockToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(2));

            await messageReceiver.DeadLetterAsync(lockToken, "Testing messages been sent to DLQ.");
        }
    }
}