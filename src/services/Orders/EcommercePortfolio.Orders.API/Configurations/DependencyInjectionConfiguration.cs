using EcommercePortfolio.Core.Mediator;
using EcommercePortfolio.Core.MessageBus;
using EcommercePortfolio.Core.Notification;
using EcommercePortfolio.ExternalServices.MyFakePayApi;
using EcommercePortfolio.Orders.API.Applications.Queries;
using EcommercePortfolio.Orders.Domain;
using EcommercePortfolio.Orders.Domain.ApiServices;
using EcommercePortfolio.Orders.Infra.ApiServices;
using EcommercePortfolio.Orders.Infra.Data;
using EcommercePortfolio.Services.Caching;

namespace EcommercePortfolio.Orders.API.Configurations;

public static class DependencyInjectionConfiguration
{
    public static void AddDependencyInjections(this IServiceCollection services)
    {
        services.AddScoped<INotificationContext, NotificationContext>();
        services.AddScoped<IMediatorHandler, MediatorHandler>();
        services.AddScoped<IMessageBus, MessageBus>();

        services.AddScoped<IOrderQueries, OrderQueries>();

        services.AddScoped<ICartApiService, CartApiService>();
        services.AddScoped<IPaymentApiService, PaymentApiService>();

        services.AddScoped<IPaymentService, PaymentService>();

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IRedisRepository, RedisRepository>();
        
        services.AddScoped<OrderPostgresDbContext>();
    }
}
