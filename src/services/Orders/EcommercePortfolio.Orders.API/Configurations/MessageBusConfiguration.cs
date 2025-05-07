using EcommercePortfolio.Services.Configurations;
using MassTransit;

namespace EcommercePortfolio.Orders.API.Configurations;

public static class MessageBusConfiguration
{
    public static void AddMessageBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            busConfigurator.UsingRabbitMq((context, config) =>
            {
                // Disables exchange creation for base types
                config.MessageTopology.SetEntityNameFormatter(new CustomEntityNameFormatter());

                config.Host(configuration.GetConnectionString("ecommerceportfolio-rabbit-mq"));
            });
        });
    }
}
