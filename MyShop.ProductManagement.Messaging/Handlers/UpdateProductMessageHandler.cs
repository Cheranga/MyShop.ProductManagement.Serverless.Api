﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        private readonly IOrderedMessagePublisher _serviceBusMessagePublisher;
        private readonly ILogger<UpdateProductMessageHandler> _logger;

        public UpdateProductMessageHandler(IOrderedMessagePublisher serviceBusMessagePublisher, ILogger<UpdateProductMessageHandler> logger)
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
}
