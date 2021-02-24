﻿using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MyShop.ProductManagement.Application.Interfaces;
using MyShop.ProductManagement.Application.Requests;
using MyShop.ProductManagement.Application.Responses;
using MyShop.ProductManagement.Serverless.Api.Dto;
using MyShop.ProductManagement.Serverless.Api.Extensions;

namespace MyShop.ProductManagement.Serverless.Api.Functions
{
    public class GetProductFunction
    {
        private readonly ILogger<GetProductFunction> _logger;
        private readonly IGetProductService _productService;

        public GetProductFunction(IGetProductService productService, ILogger<GetProductFunction> logger)
        {
            _productService = productService;
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

            var getProductRequest = new GetProductByCodeRequest
            {
                CorrelationId = correlationId,
                ProductCode = productCode
            };

            var operation = await _productService.GetProductAsync(getProductRequest);
            if (!operation.Status)
            {
                _logger.LogError("Error when getting product data {correlationId}", correlationId);

                var errorResponse = new ErrorResponse
                {
                    Message = "Error when getting the product.",
                    Errors = operation.Validation.Errors.Select(x => x.ErrorMessage).ToList()
                };

                return new ObjectResult(errorResponse)
                {
                    StatusCode = (int)(HttpStatusCode.BadRequest)
                };
            }

            var product = operation.Data;
            if (product == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(operation.Data);
        }
    }
}