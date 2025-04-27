using EcommercePortfolio.Application.Carts.Queries;
using EcommercePortfolio.Application.Orders.Queries;
using EcommercePortfolio.Core.Mediator;
using EcommercePortfolio.Core.MessageBus;
using EcommercePortfolio.Core.Notification;
using EcommercePortfolio.Domain.Caching;
using EcommercePortfolio.Domain.Carts;
using EcommercePortfolio.Domain.Deliveries;
using EcommercePortfolio.Domain.Deliveries.ApiServices;
using EcommercePortfolio.Domain.Orders;
using EcommercePortfolio.Domain.Orders.ApiServices;
using EcommercePortfolio.Domain.Payments;
using EcommercePortfolio.Domain.Products;
using EcommercePortfolio.Infra.ApiServices;
using EcommercePortfolio.Infra.Data.Caching;
using EcommercePortfolio.Infra.Data.Carts;
using EcommercePortfolio.Infra.Data.Deliveries;
using EcommercePortfolio.Infra.Data.Orders;

namespace EcommercePortfolio.API.Configurations;

public static class DependencyInjectionConfiguration
{
    public static void AddDependencyInjections(this IServiceCollection services)
    {
        services.AddScoped<INotificationContext, NotificationContext>();
        services.AddScoped<IMediatorHandler, MediatorHandler>();
        services.AddScoped<IMessageBus, MessageBus>();

        services.AddScoped<ICartQueries, CartQueries>();
        services.AddScoped<IOrderQueries, OrderQueries>();

        services.AddScoped<IFakeStoreApiService, FakeStoreApiService>();
        services.AddScoped<IPaymentApiService, PaymentApiService>();
        services.AddScoped<ICartApiService, CartApiService>();

        services.AddScoped<IPaymentService, PaymentService>();

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IDeliveryRepository, DeliveryRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IRedisRepository, RedisRepository>();
        
        services.AddScoped<OrderPostgresDbContext>();
        services.AddScoped<DeliveryPostgresDbContext>();
    }
}
