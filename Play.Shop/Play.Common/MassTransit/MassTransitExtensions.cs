using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Play.Common.Settings;

namespace Play.Common.MassTransit
{

    public static class MassTransitExtensions
    {
        public static IServiceCollection AddMassTransitWithRabbitMq(this IServiceCollection services)
        {
            services.AddMassTransit(busConfigurator => 
            {
                busConfigurator.AddConsumers(Assembly.GetEntryAssembly());

                busConfigurator.UsingRabbitMq((context, configurator) => 
                {
                    var configuration = context.GetService<IConfiguration>();
                    var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
                    var rabbitMQSettings = configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
                    
                    configurator.Host(rabbitMQSettings.Host);
                    configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(serviceSettings.ServiceName, false));

                    configurator.UseMessageRetry(retryConfigurator => 
                    {
                        retryConfigurator.Interval(3, TimeSpan.FromSeconds(5));
                    });
                });
            });

            //services.AddMassTransitHostedService();


            return services;
        }
    }
}