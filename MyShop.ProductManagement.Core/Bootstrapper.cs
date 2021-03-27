using MediatR;
using MediatR.Pipeline;
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

            services.AddTransient(typeof(IRequestPreProcessor<>), typeof(ProductsRequestPreProcessor<>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
            services.AddTransient(typeof(IRequestPostProcessor<,>), typeof(ProductsRequestPostProcessor<,>));

            return services;
        }
    }
}