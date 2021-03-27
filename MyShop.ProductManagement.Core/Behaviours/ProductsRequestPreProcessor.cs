using System.Threading;
using System.Threading.Tasks;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using MyShop.ProductManagement.Domain.Validators;

namespace MyShop.ProductManagement.Domain.Behaviours
{
    public class ProductsRequestPreProcessor<TRequest> : IRequestPreProcessor<TRequest> where TRequest:class, IValidatableRequest
    {
        private readonly ILogger<ProductsRequestPreProcessor<TRequest>> _logger;

        public ProductsRequestPreProcessor(ILogger<ProductsRequestPreProcessor<TRequest>> logger)
        {
            _logger = logger;
        }

        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                return Task.CompletedTask;
            }

            _logger.LogInformation("Started {request} with {correlationId}", typeof(TRequest).Name, request.CorrelationId);

            return Task.CompletedTask;
        }
    }
}