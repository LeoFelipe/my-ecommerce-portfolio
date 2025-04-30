using EcommercePortfolio.Carts.API.Application.Consumers;
using EcommercePortfolio.Services.Configurations;
using MassTransit;

namespace EcommercePortfolio.Carts.API.Configurations;

public static class MessageBusConfiguration
{
    public static void AddMessageBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            busConfigurator.AddConsumer<RemoveCartConsumer>();

            busConfigurator.UsingRabbitMq((context, config) =>
            {
                // Disables exchange creation for base types
                config.MessageTopology.SetEntityNameFormatter(new CustomEntityNameFormatter());

                config.Host(configuration.GetConnectionString("RabbitMqConnection"));

                config.ReceiveEndpoint("remove-cart-for-order-authorized", endpoint =>
                {
                    endpoint.ConfigureConsumer<RemoveCartConsumer>(context);
                });

                config.ConfigureEndpoints(context);
            });
        });
    }
}
