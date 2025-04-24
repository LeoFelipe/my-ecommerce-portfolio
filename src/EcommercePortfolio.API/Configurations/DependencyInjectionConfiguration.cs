using EcommercePortfolio.Application.Carts.Queries;
using EcommercePortfolio.Core.Messaging;
using EcommercePortfolio.Core.Notification;
using EcommercePortfolio.Domain.Caching;
using EcommercePortfolio.Domain.Carts;
using EcommercePortfolio.Domain.Orders;
using EcommercePortfolio.Domain.Payments;
using EcommercePortfolio.Domain.Products;
using EcommercePortfolio.Infra.ApiServices;
using EcommercePortfolio.Infra.Data.Caching;
using EcommercePortfolio.Infra.Data.Carts;
using EcommercePortfolio.Infra.Data.Orders;

namespace EcommercePortfolio.API.Configurations;

public static class DependencyInjectionConfiguration
{
    public static void AddDependencyInjections(this IServiceCollection services)
    {
        services.AddScoped<IMediatorHandler, MediatorHandler>();
        services.AddScoped<INotificationContext, NotificationContext>();

        services.AddScoped<ICartQueries, CartQueries>();

        services.AddScoped<IFakeStoreApiService, FakeStoreApiService>();
        services.AddScoped<IPaymentApiService, PaymentApiService>();

        services.AddScoped<IPaymentService, PaymentService>();

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IRedisRepository, RedisRepository>();
    }
}
