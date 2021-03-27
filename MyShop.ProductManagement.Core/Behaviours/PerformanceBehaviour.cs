using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using MyShop.ProductManagement.Domain.Validators;

namespace MyShop.ProductManagement.Domain.Behaviours
{
    public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IValidatableRequest
    {
        private readonly ILogger<PerformanceBehaviour<TRequest, TResponse>> _logger;

        public PerformanceBehaviour(ILogger<PerformanceBehaviour<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var response = await next();

            stopWatch.Stop();

            _logger.LogInformation("Handling of {request} with {correlationId} ended, time taken {timeTaken} ms.", typeof(TRequest).Name, request.CorrelationId, stopWatch.ElapsedMilliseconds);

            return response;
        }
    }
}