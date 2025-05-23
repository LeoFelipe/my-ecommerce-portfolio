﻿using EcommercePortfolio.Core.Mediator;
using EcommercePortfolio.Core.MessageBus;
using EcommercePortfolio.Core.Notification;
using EcommercePortfolio.Services.Caching;
using EcommercePortfolio.Deliveries.Domain;
using EcommercePortfolio.Deliveries.Infra.Data;
using EcommercePortfolio.Deliveries.API.Application.Queries;

namespace EcommercePortfolio.Deliveries.API.Configurations;

public static class DependencyInjectionConfiguration
{
    public static void AddDependencyInjections(this IServiceCollection services)
    {
        services.AddScoped<INotificationContext, NotificationContext>();
        services.AddScoped<IRedisRepository, RedisRepository>();
        services.AddScoped<IMediatorHandler, MediatorHandler>();
        services.AddScoped<IMessageBus, MessageBus>();

        services.AddScoped<DeliveryPostgresDbContext>();

        services.AddScoped<IDeliveryRepository, DeliveryRepository>();
        services.AddScoped<IDeliveryQueries, DeliveryQueries>();

    }
}
