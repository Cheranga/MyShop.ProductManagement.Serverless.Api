using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using MyShop.ProductManagement.Application.Messages;
using MyShop.ProductManagement.Domain;

namespace MyShop.ProductManagement.Messaging.Handlers
{
    public class CreateProductMessageHandler : IRequestHandler<CreateProductMessage, Result>
    {
        private readonly ILogger<CreateProductMessageHandler> _logger;
        private readonly IOrderedMessagePublisher _serviceBusMessagePublisher;

        public CreateProductMessageHandler(IOrderedMessagePublisher serviceBusMessagePublisher, ILogger<CreateProductMessageHandler> logger)
        {
            _serviceBusMessagePublisher = serviceBusMessagePublisher;
            _logger = logger;
        }

        public Task<Result> Handle(CreateProductMessage request, CancellationToken cancellationToken)
        {
            var message = new CreateProductServiceBusMessage
            {
                CorrelationId = request.CorrelationId,
                ProductCode = request.ProductCode,
                ProductName = request.ProductName
            };

            return _serviceBusMessagePublisher.PublishAsync(message);
        }
    }
}