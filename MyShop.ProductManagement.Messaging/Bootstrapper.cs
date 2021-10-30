using Azure.Identity;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyShop.ProductManagement.Messaging.Configs;
using MyShop.ProductManagement.Messaging.Handlers;

namespace MyShop.ProductManagement.Messaging
{
    public static class Bootstrapper
    {
        public static IServiceCollection UseMessagingServices(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.Configure<ServiceBusConfig>(configuration.GetSection(nameof(ServiceBusConfig)));
            services.AddScoped(provider =>
            {
                var config = provider.GetRequiredService<IOptionsSnapshot<ServiceBusConfig>>().Value;
                return config;
            });

            services.AddAzureClients(x =>
            {
                x.AddServiceBusClient(configuration.GetSection(nameof(ServiceBusConfig)))
                    .WithCredential(new DefaultAzureCredential());
            });
            // services.AddSingleton<ITopicClient, TopicClient>(provider =>
            // {
            //     var serviceBusConfig = provider.GetRequiredService<ServiceBusConfig>();
            //     new ServiceBusClient()
            //     return new TopicClient(serviceBusConfig.WriteConnectionString, serviceBusConfig.WriteTopic);
            // });

            services.AddTransient<IOrderedMessagePublisher, OrderedMessagePublisher>();

            return services;
        }
    }
}