using EcommercePortfolio.Core.Notification;
using EcommercePortfolio.Domain.Orders;
using EcommercePortfolio.Domain.Products;
using EcommercePortfolio.Infra.ApiServices;
using EcommercePortfolio.Infra.Orders;

namespace EcommercePortfolio.API.Configurations;

public static class DependencyInjectionConfiguration
{
    public static void AddDependencyInjections(this IServiceCollection services)
    {
        services.AddScoped<IFakeStoreApiService, FakeStoreApiService>();
        services.AddScoped<INotificationHandler, NotificationHandler>();
        services.AddScoped<IOrderRepository, OrderRepository>();
    }
}
