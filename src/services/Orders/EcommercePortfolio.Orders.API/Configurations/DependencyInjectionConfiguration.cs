﻿using EcommercePortfolio.Core.Mediator;
using EcommercePortfolio.Core.MessageBus;
using EcommercePortfolio.Core.Notification;
using EcommercePortfolio.ApiGateways.MyFakePaymentApi;
using EcommercePortfolio.Orders.API.Application.Queries;
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
        services.AddScoped<IRedisRepository, RedisRepository>();
        services.AddScoped<IMediatorHandler, MediatorHandler>();
        services.AddScoped<IMessageBus, MessageBus>();

        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IPaymentApiService, PaymentApiService>();

        services.AddScoped<OrderPostgresDbContext>();

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderQueries, OrderQueries>();
        services.AddScoped<ICartApiService, CartApiService>();

    }
}
