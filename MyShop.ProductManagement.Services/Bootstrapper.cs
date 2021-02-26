using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MyShop.ProductManagement.Application
{
    public static class Bootstrapper
    {
        public static void UseProductsServices(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
            {
                return;
            }
            // Register application level dependencies here.
        }
    }
}