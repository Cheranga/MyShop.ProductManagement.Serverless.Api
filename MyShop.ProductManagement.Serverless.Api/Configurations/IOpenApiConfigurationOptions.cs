using Microsoft.OpenApi.Models;

namespace MyShop.ProductManagement.Serverless.Api.Configurations
{
    public interface IOpenApiConfigurationOptions
    {
        /// <summary>
        ///     Gets or sets the <see cref="OpenApiInfo" /> instance.
        /// </summary>
        OpenApiInfo Info { get; set; }
    }
}