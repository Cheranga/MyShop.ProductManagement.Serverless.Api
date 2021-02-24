using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyShop.ProductManagement.Application.Interfaces;
using MyShop.ProductManagement.Application.Services;
using MyShop.ProductManagement.Domain.Validators;

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

            services.AddScoped<IGetProductService, GetProductService>();
            services.AddScoped<IUpsertProductService, UpsertProductService>();
        }
    }
}