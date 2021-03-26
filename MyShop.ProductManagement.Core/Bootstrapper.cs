using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MyShop.ProductManagement.Domain.Behaviours;

namespace MyShop.ProductManagement.Domain
{
    public static class Bootstrapper
    {
        public static IServiceCollection RegisterDomainServices(this IServiceCollection services)
        {
            if (services == null)
            {
                return services;
            }

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));

            return services;
        }
    }
}