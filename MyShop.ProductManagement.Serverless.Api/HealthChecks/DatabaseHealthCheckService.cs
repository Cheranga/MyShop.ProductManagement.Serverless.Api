using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MyShop.ProductManagement.Application.Interfaces;
using MyShop.ProductManagement.Application.Requests;
using MyShop.ProductManagement.DataAccess;

namespace MyShop.ProductManagement.Serverless.Api.HealthChecks
{
    public class DatabaseHealthCheckService : IHealthCheck
    {
        private readonly IGetProductService _getProductService;

        public DatabaseHealthCheckService(IGetProductService getProductService)
        {
            _getProductService = getProductService;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            var getProductByCodeRequest = new GetProductByCodeRequest
            {
                CorrelationId = "HEALTH_CHECK",
                ProductCode = "BLAH"
            };

            var operation = await _getProductService.GetProductAsync(getProductByCodeRequest);

            if (operation.Status)
            {
                return HealthCheckResult.Healthy();
            }

            var errorData = new Dictionary<string, object>
            {
                {"Message", "Cannot access product data." }
            };

            return HealthCheckResult.Unhealthy(description:"Accessing product data", data:errorData);
        }
    }
}