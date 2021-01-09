using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MyShop.ProductManagement.Serverless.Api.Functions
{
    public class UpsertProductFunction
    {
        public UpsertProductFunction(ILogger<UpsertProductFunction> logger)
        {
            
        }

        [FunctionName(nameof(UpsertProductFunction))]
        public async Task<IActionResult> UpsertProductAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "products")]
            HttpRequest request)
        {
            var content = await new StreamReader(request.Body).ReadToEndAsync();
            
            return new OkResult();
        }
    }
}
