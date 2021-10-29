using System.Net;
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

namespace MyShop.ProductManagement.Serverless.Api.Functions.V2
{
    public class CreateProductFunction
    {
        private readonly ILogger<CreateProductFunction> _logger;
        private readonly IMediator _mediator;
        private readonly IRenderAction<InsertProductDto, Result<GetProductResponse>> _responseFormatter;

        public CreateProductFunction(IMediator mediator, IRenderAction<InsertProductDto, Result<GetProductResponse>> responseFormatter, ILogger<CreateProductFunction> logger)
        {
            _mediator = mediator;
            _responseFormatter = responseFormatter;
            _logger = logger;
        }

        [FunctionName(nameof(CreateProductFunction))]
        [OpenApiOperation("InsertProduct", "product", Summary = "Insert product.", Description = "This will insert a new product.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody("application/json", typeof(InsertProductDto), Required = true, Description = "The product data which needs to be inserted.")]
        [OpenApiParameter("correlationId", In = ParameterLocation.Header, Required = true, Type = typeof(string), Summary = "Correlation id", Description = "This will be used to track the operation", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(GetProductResponse), Summary = "The product details.", Description = "The inserted product details.")]
        [OpenApiResponseWithoutBody(HttpStatusCode.BadRequest, Summary = "Invalid input", Description = "Invalid input")]
        public async Task<IActionResult> InsertProductAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v2/products")]
            HttpRequest request)
        {
            var correlationId = request.GetHeaderValue("correlationId");

            var dto = await request.ToModel<InsertProductDto>();
            dto.CorrelationId = correlationId;

            var operation = await _mediator.Send(dto);

            if (!operation.Status)
            {
                _logger.LogError("Error occured when upserting product {correlationId}", correlationId);
            }

            var response = _responseFormatter.Render(dto, operation);
            return response;
        }
    }
}