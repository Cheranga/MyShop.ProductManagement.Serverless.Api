using FluentValidation;
using MediatR;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyShop.ProductManagement.Application;
using MyShop.ProductManagement.Application.Responses;
using MyShop.ProductManagement.DataAccess;
using MyShop.ProductManagement.Domain;
using MyShop.ProductManagement.Messaging;
using MyShop.ProductManagement.Serverless.Api;
using MyShop.ProductManagement.Serverless.Api.Dto;
using MyShop.ProductManagement.Serverless.Api.HealthChecks;
using MyShop.ProductManagement.Serverless.Api.ResponseFormatters;
using Serilog;
using Serilog.Events;
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

            RegisterApiServices(services)
                .UseProductsServices(configuration)
                .UseProductsDataAccess(configuration)
                .UseMessagingServices(configuration)
                .RegisterDomainServices();
        }

        private static IServiceCollection RegisterApiServices(IServiceCollection services)
        {
            RegisterLogging(services);
            RegisterResponseFormatters(services);
            RegisterMediators(services);
            RegisterHealthChecks(services);
            RegisterValidators(services);

            return services;
        }

        private static void RegisterLogging(IServiceCollection services)
        {
            services.AddLogging(builder =>
            {
                var logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                    .WriteTo.ApplicationInsights(TelemetryConfiguration.CreateDefault(), TelemetryConverter.Traces, LogEventLevel.Debug)
                    .CreateLogger();

                builder.AddSerilog(logger);
            });
        }

        private static void RegisterValidators(IServiceCollection services)
        {
            var validatorAssemblies = new[]
            {
                typeof(Startup).Assembly,
                typeof(Bootstrapper).Assembly,
                typeof(DataAccess.Bootstrapper).Assembly
            };
            services.AddValidatorsFromAssemblies(validatorAssemblies);
        }

        private static void RegisterHealthChecks(IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddCheck<DatabaseHealthCheckService>("Database health check.");
        }

        private static void RegisterMediators(IServiceCollection services)
        {
            var mediatorAssemblies = new[]
            {
                typeof(Startup).Assembly, 
                typeof(Bootstrapper).Assembly, 
                typeof(DataAccess.Bootstrapper).Assembly,
                typeof(Messaging.Bootstrapper).Assembly
            };
            services.AddMediatR(mediatorAssemblies);
        }

        private static void RegisterResponseFormatters(IServiceCollection services)
        {
            services.AddTransient<IRenderAction<GetProductByCodeDto, Result<GetProductResponse>>, DisplayProductFormatter>();
            services.AddTransient<IRenderAction<UpsertProductDto, Result<GetProductResponse>>, UpsertProductFormatter>();
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