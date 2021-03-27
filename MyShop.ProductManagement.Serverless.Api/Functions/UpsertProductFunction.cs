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
using MyShop.ProductManagement.Application.Requests;
using MyShop.ProductManagement.Application.Responses;
using MyShop.ProductManagement.Domain;
using MyShop.ProductManagement.Serverless.Api.Dto;
using MyShop.ProductManagement.Serverless.Api.Extensions;
using MyShop.ProductManagement.Serverless.Api.ResponseFormatters;

namespace MyShop.ProductManagement.Serverless.Api.Functions
{
    public class UpsertProductFunction
    {
        private readonly ILogger<UpsertProductFunction> _logger;
        private readonly IMediator _mediator;
        private readonly IRenderAction<UpsertProductDto, Result<GetProductResponse>> _responseFormatter;

        public UpsertProductFunction(IMediator mediator, IRenderAction<UpsertProductDto, Result<GetProductResponse>> responseFormatter, ILogger<UpsertProductFunction> logger)
        {
            _mediator = mediator;
            _responseFormatter = responseFormatter;
            _logger = logger;
        }

        [FunctionName(nameof(UpsertProductFunction))]
        [OpenApiOperation("UpsertProduct", "product", Summary = "Insert or update product.", Description = "This will insert a new product or will update an existing product.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody("application/json", typeof(UpsertProductRequest), Required = true, Description = "The product data which needs to be inserted as a new product or to be updated if it's an existing product.")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(GetProductResponse), Summary = "The product details.", Description = "The product details which was either inserted or updated.")]
        [OpenApiResponseWithoutBody(HttpStatusCode.MethodNotAllowed, Summary = "Invalid input", Description = "Invalid input")]
        public async Task<IActionResult> UpsertProductAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "products")]
            HttpRequest request)
        {
            var correlationId = request.GetHeaderValue("correlationId");

            var dto = await request.ToModel<UpsertProductDto>();
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