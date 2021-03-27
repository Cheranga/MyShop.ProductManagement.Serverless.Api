using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;

namespace MyShop.ProductManagement.Serverless.Api.Functions.V1
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
        [OpenApiParameter("correlationId", In = ParameterLocation.Header, Required = true, Type = typeof(string), Summary = "Correlation id", Description = "This will be used to track the operation", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithoutBody(HttpStatusCode.OK, Summary = "API is healthy.", Description = "The health of the API is healthy.")]
        [OpenApiResponseWithoutBody(HttpStatusCode.InternalServerError, Summary = "API is unhealthy.", Description = "The API is not functioning successfully.")]
        public async Task<IActionResult> Health([HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethod.Get), Route = "v1/health")]
            HttpRequest request)
        {
            var healthCheckResponse = await _healthCheckService.CheckHealthAsync();
            var healthStatus = healthCheckResponse.Status;

            if (healthStatus == HealthStatus.Healthy || healthStatus == HealthStatus.Degraded)
            {
                return new OkResult();
            }

            return new ObjectResult(healthCheckResponse)
            {
                StatusCode = (int) HttpStatusCode.InternalServerError
            };
        }
    }
}