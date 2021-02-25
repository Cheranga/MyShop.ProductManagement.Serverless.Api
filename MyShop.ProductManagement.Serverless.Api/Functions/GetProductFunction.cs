using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MyShop.ProductManagement.Application.Responses;
using MyShop.ProductManagement.Domain;
using MyShop.ProductManagement.Serverless.Api.Dto;
using MyShop.ProductManagement.Serverless.Api.Extensions;
using MyShop.ProductManagement.Serverless.Api.ResponseFormatters;

namespace MyShop.ProductManagement.Serverless.Api.Functions
{
    public class GetProductFunction
    {
        private readonly ILogger<GetProductFunction> _logger;
        private readonly IMediator _mediator;
        private readonly IRenderAction<GetProductByCodeDto, Result<Product>> _responseFormatter;

        public GetProductFunction(IMediator mediator, IRenderAction<GetProductByCodeDto, Result<Product>> responseFormatter, ILogger<GetProductFunction> logger)
        {
            _mediator = mediator;
            _responseFormatter = responseFormatter;
            _logger = logger;
        }

        [FunctionName(nameof(GetProductFunction))]
        [OpenApiOperation("getProductByProductCode", "product", Summary = "Get product by product code", Description = "Get the product using product code.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter("productCode", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "Product code", Description = "Need the product code to find the product", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(GetProductResponse), Summary = "successful operation", Description = "The product data.")]
        [OpenApiResponseWithoutBody(HttpStatusCode.NotFound, Summary = "unsuccessful operation", Description = "The product is not found.")]
        public async Task<IActionResult> GetProductAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "products/{productCode}")]
            HttpRequest request, string productCode)
        {
            var correlationId = request.GetHeaderValue("correlationId");
            if (string.IsNullOrWhiteSpace(correlationId))
            {
                return new BadRequestObjectResult("correlationId is required in the HTTP header.");
            }

            var dto = new GetProductByCodeDto
            {
                CorrelationId = correlationId,
                ProductCode = productCode
            };

            var cancellationToken = new CancellationTokenSource().Token;
            var operation = await _mediator.Send(dto, cancellationToken);

            if (!operation.Status)
            {
                _logger.LogError("Error occured when getting product: {correlationId}", correlationId);
            }

            var result = _responseFormatter.Render(dto, operation);
            return result;
        }
    }
}