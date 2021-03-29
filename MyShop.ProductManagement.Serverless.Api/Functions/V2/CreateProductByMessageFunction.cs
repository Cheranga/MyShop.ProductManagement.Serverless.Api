using System.Threading.Tasks;
using MediatR;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using MyShop.ProductManagement.Application.DataAccess;
using MyShop.ProductManagement.Messaging.Handlers;
using MyShop.ProductManagement.Serverless.Api.Services;

namespace MyShop.ProductManagement.Serverless.Api.Functions.V2
{
    public class CreateProductByMessageFunction
    {
        private readonly IMediator _mediator;
        private readonly IMessageReader _messageReader;

        public CreateProductByMessageFunction(IMediator mediator, IMessageReader messageReader)
        {
            _mediator = mediator;
            _messageReader = messageReader;
        }

        [FunctionName(nameof(CreateProductByMessageFunction))]
        public async Task Run(
            [ServiceBusTrigger("%ServiceBusConfig:WriteTopic%", "%ServiceBusConfig:CreateProductSubscription%", Connection = "ServiceBusConfig:ReadConnectionString", IsSessionsEnabled = true)]
            Message message,
            IMessageSession messageSession, string lockToken)
        {
            var operation = await _messageReader.GetModelAsync<CreateProductServiceBusMessage>(message);
            if (!operation.Status)
            {
                await messageSession.DeadLetterAsync(lockToken, operation.ErrorCode);
                return;
            }

            var model = operation.Data;
            var insertProductCommand = new InsertProductCommand(model.CorrelationId, model.ProductCode, model.ProductName);
            var insertOperation = await _mediator.Send(insertProductCommand);

            if (!insertOperation.Status)
            {
                await messageSession.DeadLetterAsync(lockToken, insertOperation.ErrorCode);
                return;
            }

            await messageSession.CompleteAsync(lockToken);
        }
    }
}