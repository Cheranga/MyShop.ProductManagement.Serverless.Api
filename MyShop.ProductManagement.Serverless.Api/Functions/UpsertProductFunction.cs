using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using MyShop.ProductManagement.Api.Services;
using MyShop.ProductManagement.Serverless.Api.Extensions;
using MyShop.ProductManagement.Services.Requests;
using Newtonsoft.Json;

namespace MyShop.ProductManagement.Serverless.Api.Functions
{
    public class UpsertProductFunction
    {
        private readonly ILogger<UpsertProductFunction> _logger;
        private readonly IProductsService _productsService;

        public UpsertProductFunction(IProductsService productsService, ILogger<UpsertProductFunction> logger)
        {
            _productsService = productsService;
            _logger = logger;
        }

        [FunctionName(nameof(UpsertProductFunction))]
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

            return new InternalServerErrorResult();
        }
    }
}