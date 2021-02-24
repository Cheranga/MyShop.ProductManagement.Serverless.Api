using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MyShop.ProductManagement.Serverless.Api.Functions
{
    public class HealthCheckFunction
    {
        private readonly HealthCheckService _healthCheckService;

        public HealthCheckFunction(HealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        [FunctionName(nameof(HealthCheckFunction))]
        [OpenApiOperation("health", "product", Summary = "Get the health status of the API.", Description = "After various health checks have been conducted will return the health of the API.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithoutBody(HttpStatusCode.OK, Summary = "API is healthy.", Description = "The health of the API is healthy.")]
        [OpenApiResponseWithoutBody(HttpStatusCode.InternalServerError, Summary = "API is unhealthy.", Description = "The API is not functioning successfully.")]
        //[OpenApiResponseWithBody(HttpStatusCode.InternalServerError, "application/json", typeof(HealthCheckResult), Summary = "API is unhealthy.", Description = "The API is not functioning successfully.")]
        public async Task<IActionResult> Health([HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethod.Get), Route = "health")]
            HttpRequest request)
        {
            var healthCheckResponse = await _healthCheckService.CheckHealthAsync();
            var healthStatus = healthCheckResponse.Status;

            if (healthStatus == HealthStatus.Healthy || healthStatus == HealthStatus.Degraded)
            {
                return new OkResult();
            }

            return new InternalServerErrorResult();
        }
    }
}