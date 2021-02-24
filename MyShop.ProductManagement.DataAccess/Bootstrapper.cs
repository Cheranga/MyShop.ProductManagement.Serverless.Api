using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyShop.ProductManagement.Application.Interfaces;
using MyShop.ProductManagement.DataAccess.Behaviours;
using MyShop.ProductManagement.DataAccess.Services;
using MyShop.ProductManagement.Domain.Validators;

namespace MyShop.ProductManagement.DataAccess
{
    public static class Bootstrapper
    {
        public static void UseProductsDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
            {
            }

            services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
            services.Configure<DatabaseConfig>(configuration.GetSection("DatabaseConfig"));
            services.AddScoped(provider =>
            {
                var config = provider.GetRequiredService<IOptionsSnapshot<DatabaseConfig>>().Value;
                return config;
            });

            services.AddScoped<IGetProductDataService, GetProductDataService>();
            services.AddScoped<IUpsertProductDataService, UpsertProductDataService>();
        }
    }
}