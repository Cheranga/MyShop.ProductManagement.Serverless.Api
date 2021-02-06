using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.OpenApi.Models;

namespace MyShop.ProductManagement.Serverless.Api.Configurations
{
    public class OpenApiConfigurationOptions : IOpenApiConfigurationOptions
    {
        public OpenApiInfo Info { get; set; } = new OpenApiInfo()
        {
            Version = "3.0.0",
            Title = "Open API Sample on Azure Functions (IoC)",
            Description = "A sample API that runs on Azure Functions (IoC) 3.x using Open API specification.",
            TermsOfService = new Uri("https://github.com/Azure/azure-functions-openapi-extension"),
            Contact = new OpenApiContact()
            {
                Name = "Contoso",
                Email = "azfunc-openapi@contoso.com",
                Url = new Uri("https://github.com/Azure/azure-functions-openapi-extension/issues"),
            },
            License = new OpenApiLicense()
            {
                Name = "MIT",
                Url = new Uri("http://opensource.org/licenses/MIT"),
            }
        };
    }
}
