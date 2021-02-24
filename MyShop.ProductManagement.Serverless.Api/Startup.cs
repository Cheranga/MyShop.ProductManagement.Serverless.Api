using FluentValidation;
using MediatR;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyShop.ProductManagement.Application;
using MyShop.ProductManagement.DataAccess;
using MyShop.ProductManagement.DataAccess.Behaviours;
using MyShop.ProductManagement.Domain.Validators;
using MyShop.ProductManagement.Serverless.Api;
using MyShop.ProductManagement.Serverless.Api.HealthChecks;
using Bootstrapper = MyShop.ProductManagement.Application.Bootstrapper;

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
            services.UseProductsDataAccess(configuration);

            services.AddHealthChecks()
                .AddCheck<DatabaseHealthCheckService>("Database health check.");

            services.AddValidatorsFromAssemblies(new[] {typeof(Application.Bootstrapper).Assembly, typeof(DataAccess.Bootstrapper).Assembly});

            services.AddMediatR(typeof(Application.Bootstrapper), typeof(DataAccess.Bootstrapper));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        }

        protected virtual IConfigurationRoot GetConfigurationRoot(IFunctionsHostBuilder builder)
        {
            var services = builder.Services;

            var executionContextOptions = services.BuildServiceProvider().GetService<IOptions<ExecutionContextOptions>>().Value;

            var configuration = new ConfigurationBuilder()
                .SetBasePath(executionContextOptions.AppDirectory)
                .AddJsonFile("local.settings.json", true, true)
                .AddEnvironmentVariables()
                .Build();

            return configuration;
        }
    }
}