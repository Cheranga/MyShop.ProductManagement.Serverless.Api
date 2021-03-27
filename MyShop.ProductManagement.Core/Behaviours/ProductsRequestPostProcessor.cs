using System.Threading;
using System.Threading.Tasks;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using MyShop.ProductManagement.Domain.Validators;

namespace MyShop.ProductManagement.Domain.Behaviours
{
    public class ProductsRequestPostProcessor<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse> where TRequest : class, IValidatableRequest
    {
        private readonly ILogger<ProductsRequestPostProcessor<TRequest, TResponse>> _logger;

        public ProductsRequestPostProcessor(ILogger<ProductsRequestPostProcessor<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                return Task.CompletedTask;
            }

            _logger.LogInformation("Finished {request} with {correlationId}", typeof(TRequest).Name, request.CorrelationId);

            return Task.CompletedTask;
        }
    }
}