using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace MyShop.ProductManagement.Serverless.Api.Functions
{
    public class TestFunction
    {
        [FunctionName(nameof(TestFunction))]
        public Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethod.Get), Route = "test")]
            HttpRequest request)
        {
            var actionResult = (IActionResult) (new OkResult());
            return Task.FromResult(actionResult);
        }
    }
}
