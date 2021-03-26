using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MyShop.ProductManagement.Application
{
    public static class Bootstrapper
    {
        public static IServiceCollection UseProductsServices(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
            {
                return services;
            }
            // Register application level dependencies here.

            return services;
        }
    }
}