using MediatR;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyShop.ProductManagement.Serverless.Api;
using MyShop.ProductManagement.Services;

[assembly: FunctionsStartup(typeof(Startup))]

namespace MyShop.ProductManagement.Serverless.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var services = builder.Services;

            var configuration = GetConfigurationRoot(builder);

            services.UseProductsServices(configuration);

            services.AddMediatR(typeof(Startup).Assembly, typeof(Bootstrapper).Assembly, typeof(DataAccess.Bootstrapper).Assembly);
        }

        protected virtual IConfigurationRoot GetConfigurationRoot(IFunctionsHostBuilder builder)
        {
            var services = builder.Services;

            var executionContextOptions = services.BuildServiceProvider().GetService<IOptions<ExecutionContextOptions>>().Value;

            var configuration = new ConfigurationBuilder()
                .SetBasePath(executionContextOptions.AppDirectory)
                .AddJsonFile("local.settings.json", true)
                .AddEnvironmentVariables()
                .Build();

            return configuration;
        }


    }
}