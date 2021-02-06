using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;

namespace MyShop.ProductManagement.Serverless.Api.Functions
{
    public class SwaggerFunctions
    {
        [FunctionName(nameof(RenderOpenApiDocument))]
        [OpenApiIgnore]
        public async Task<IActionResult> RenderOpenApiDocument(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "openapi/{version}.{extension}")] HttpRequest req,
            string version,
            string extension,
            ILogger log)
        {
            //var helper = new DocumentHelper();
            var document = new Document(new DocumentHelper(new RouteConstraintFilter(),new OpenApiSchemaAcceptor() ));
            var result = await document.InitialiseDocument()
                .AddMetadata(new OpenApiInfo
                {
                    Description = "Some description goes here."
                })
                .AddServer(req, "api")
                .Build(Assembly.GetExecutingAssembly())
                .RenderAsync(OpenApiSpecVersion.OpenApi3_0,OpenApiFormat.Json)
                .ConfigureAwait(false);
            var response = new ContentResult()
            {
                Content = result,
                ContentType = "application/json",
                StatusCode = (int)HttpStatusCode.OK
            };

            return response;
        }
    }
}
