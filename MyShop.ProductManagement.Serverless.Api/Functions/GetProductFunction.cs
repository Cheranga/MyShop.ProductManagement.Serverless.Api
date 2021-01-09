using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using MyShop.ProductManagement.Api.Services;
using MyShop.ProductManagement.Services.Requests;

namespace MyShop.ProductManagement.Serverless.Api.Functions
{
    public class GetProductFunction
    {
        private readonly ILogger<GetProductFunction> _logger;
        private readonly IProductsService _productService;

        public GetProductFunction(IProductsService productService, ILogger<GetProductFunction> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [FunctionName(nameof(GetProductFunction))]
        public async Task<IActionResult> GetProductAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "products/{productCode}")]
            HttpRequest request, string productCode)
        {   
            var getProductRequest = new GetProductRequest
            {
                ProductCode = productCode
            };

            var operation = await _productService.GetProductAsync(getProductRequest);
            if (operation.Status)
            {
                return new OkObjectResult(operation.Data);
            }

            return new NotFoundResult();
        }
    }
}