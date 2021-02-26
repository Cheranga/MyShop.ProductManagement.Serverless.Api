using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MyShop.ProductManagement.Application.Requests;

namespace MyShop.ProductManagement.Serverless.Api.HealthChecks
{
    public class DatabaseHealthCheckService : IHealthCheck
    {
        private readonly IMediator _mediator;

        public DatabaseHealthCheckService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            var getProductByCodeRequest = new GetProductByCodeRequest
            {
                CorrelationId = "HEALTH_CHECK",
                ProductCode = "BLAH"
            };

            var operation = await _mediator.Send(getProductByCodeRequest, cancellationToken);

            if (operation.Status)
            {
                return HealthCheckResult.Healthy();
            }

            var errorData = new Dictionary<string, object>
            {
                {"Message", "Cannot access product data."}
            };

            return HealthCheckResult.Unhealthy("Accessing product data", data: errorData);
        }
    }
}