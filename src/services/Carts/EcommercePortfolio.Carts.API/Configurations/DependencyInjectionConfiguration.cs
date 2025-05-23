﻿using EcommercePortfolio.Carts.API.Application.Queries;
using EcommercePortfolio.Carts.Domain.Carts;
using EcommercePortfolio.Carts.Infra.Data;
using EcommercePortfolio.Core.Mediator;
using EcommercePortfolio.Core.Notification;
using EcommercePortfolio.ApiGateways.FakeStoreApi;
using EcommercePortfolio.Services.Caching;

namespace EcommercePortfolio.Carts.API.Configurations;

public static class DependencyInjectionConfiguration
{
    public static void AddDependencyInjections(this IServiceCollection services)
    {
        services.AddScoped<INotificationContext, NotificationContext>();
        services.AddScoped<IRedisRepository, RedisRepository>();
        services.AddScoped<IMediatorHandler, MediatorHandler>();

        services.AddScoped<IFakeStoreApiService, FakeStoreApiService>();

        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<ICartQueries, CartQueries>();
    }
}
