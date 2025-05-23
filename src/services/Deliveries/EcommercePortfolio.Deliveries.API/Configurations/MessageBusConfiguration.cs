using EcommercePortfolio.Deliveries.API.Application.Consumers;
using EcommercePortfolio.Services.Configurations;
using MassTransit;

namespace EcommercePortfolio.Deliveries.API.Configurations;

public static class MessageBusConfiguration
{
    public static void AddMessageBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            busConfigurator.AddConsumer<CreateDeliveryConsumer>();

            busConfigurator.UsingRabbitMq((context, config) =>
            {
                // Disables exchange creation for base types
                config.MessageTopology.SetEntityNameFormatter(new CustomEntityNameFormatter());

                var rabbitMqConnectionString = configuration.GetConnectionString("RabbitMqConnection")
                    ?? throw new InvalidOperationException("RabbitMQ connection string not found.");

                config.Host(rabbitMqConnectionString);

                config.ReceiveEndpoint("create-delivery-for-order-authorized", endpoint =>
                {
                    endpoint.ConfigureConsumer<CreateDeliveryConsumer>(context);
                });
            });
        });
    }
}
