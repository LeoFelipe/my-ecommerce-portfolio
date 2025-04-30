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
                config.Host(configuration.GetConnectionString("RabbitMqConnection"));
            });
        });
    }
}
