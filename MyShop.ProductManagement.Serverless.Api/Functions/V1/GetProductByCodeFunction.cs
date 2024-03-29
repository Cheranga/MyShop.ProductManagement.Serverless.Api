﻿using System.Net;
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

namespace MyShop.ProductManagement.Serverless.Api.Functions.V1
{
    public class GetProductByCodeFunction
    {
        private readonly ILogger<GetProductByCodeFunction> _logger;
        private readonly IMediator _mediator;
        private readonly IRenderAction<GetProductByCodeDto, Result<GetProductResponse>> _responseFormatter;

        public GetProductByCodeFunction(IMediator mediator, IRenderAction<GetProductByCodeDto, Result<GetProductResponse>> responseFormatter, ILogger<GetProductByCodeFunction> logger)
        {
            _mediator = mediator;
            _responseFormatter = responseFormatter;
            _logger = logger;
        }

        [FunctionName(nameof(GetProductByCodeFunction))]
        [OpenApiOperation("getProductByProductCode", "product", Summary = "Get product by product code", Description = "Get the product using product code.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter("productCode", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "Product code", Description = "Need the product code to find the product", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter("correlationId", In = ParameterLocation.Header, Required = true, Type = typeof(string), Summary = "Correlation id", Description = "This will be used to track the operation", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(GetProductResponse), Summary = "successful operation", Description = "The product data.")]
        [OpenApiResponseWithoutBody(HttpStatusCode.NotFound, Summary = "unsuccessful operation", Description = "The product is not found.")]
        public async Task<IActionResult> GetProductAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/products/{productCode}")]
            HttpRequest request, string productCode)
        {
            var correlationId = request.GetHeaderValue("correlationId");

            var dto = new GetProductByCodeDto
            {
                CorrelationId = correlationId,
                ProductCode = productCode
            };

            var operation = await _mediator.Send(dto);

            if (!operation.Status)
            {
                _logger.LogError("Error occured when getting product: {correlationId}", correlationId);
            }

            var result = _responseFormatter.Render(dto, operation);
            return result;
        }
    }
}