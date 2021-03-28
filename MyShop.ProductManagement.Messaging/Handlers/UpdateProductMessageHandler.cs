using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using MyShop.ProductManagement.Application.Messages;
using MyShop.ProductManagement.Domain;

namespace MyShop.ProductManagement.Messaging.Handlers
{
    public class UpdateProductMessageHandler : IRequestHandler<UpdateProductMessage, Result>
    {
        private readonly IMessagePublisher _serviceBusMessagePublisher;
        private readonly ILogger<UpdateProductMessageHandler> _logger;

        public UpdateProductMessageHandler(IMessagePublisher serviceBusMessagePublisher, ILogger<UpdateProductMessageHandler> logger)
        {
            _serviceBusMessagePublisher = serviceBusMessagePublisher;
            _logger = logger;
        }

        public Task<Result> Handle(UpdateProductMessage request, CancellationToken cancellationToken)
        {
            var message = new UpdateProductServiceBusMessage
            {
                CorrelationId = request.CorrelationId,
                ProductCode = request.ProductCode,
                ProductName = request.ProductName
            };

            return _serviceBusMessagePublisher.PublishAsync(message);
        }
    }

    public class CreateProductMessageHandler : IRequestHandler<CreateProductMessage, Result>
    {
        private readonly IMessagePublisher _serviceBusMessagePublisher;
        private readonly ILogger<CreateProductMessageHandler> _logger;

        public CreateProductMessageHandler(IMessagePublisher serviceBusMessagePublisher, ILogger<CreateProductMessageHandler> logger)
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

    public abstract class MessageBase
    {
        public string CorrelationId { get; set; }
    }


    public class UpdateProductServiceBusMessage : MessageBase
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
    }

    public class CreateProductServiceBusMessage : MessageBase
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
    }

}
