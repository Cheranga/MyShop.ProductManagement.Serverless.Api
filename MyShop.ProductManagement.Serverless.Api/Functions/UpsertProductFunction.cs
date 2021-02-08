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
using MyShop.ProductManagement.Application.Interfaces;
using MyShop.ProductManagement.Application.Requests;
using MyShop.ProductManagement.Application.Responses;
using MyShop.ProductManagement.Serverless.Api.Extensions;

namespace MyShop.ProductManagement.Serverless.Api.Functions
{
    public class UpsertProductFunction
    {
        private readonly ILogger<UpsertProductFunction> _logger;
        private readonly IUpsertProductService _productsService;

        public UpsertProductFunction(IUpsertProductService productsService, ILogger<UpsertProductFunction> logger)
        {
            _productsService = productsService;
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
            if (string.IsNullOrWhiteSpace(correlationId))
            {
                return new BadRequestObjectResult("correlationId is required in the HTTP header.");
            }

            var upsertProductRequest = await request.ToModel<UpsertProductRequest>();
            if (upsertProductRequest != null)
            {
                upsertProductRequest.CorrelationId = correlationId;
            }

            var operation = await _productsService.UpsertProductAsync(upsertProductRequest);

            if (operation.Status)
            {
                return new OkObjectResult(operation.Data);
            }

            _logger.LogError("Error occured when upserting product {correlationId}", correlationId);
            return new InternalServerErrorResult();
        }
    }
}