using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using MyShop.ProductManagement.Serverless.Api;

[assembly: FunctionsStartup(typeof(Startup))]

namespace MyShop.ProductManagement.Serverless.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            //
            // TODO: register dependencies
            //
        }
    }
}