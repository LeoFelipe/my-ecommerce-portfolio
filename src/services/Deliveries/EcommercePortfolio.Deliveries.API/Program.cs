using EcommercePortfolio.Deliveries.API.Application.Commands;
using EcommercePortfolio.Deliveries.API.Configurations;
using EcommercePortfolio.Deliveries.Infra.Data;
using EcommercePortfolio.Services.Filters;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Scalar.AspNetCore;
using System.Text.Json;

// TO DO: Create a new queue so that when the order is authorized, the order can be sent for delivery tracking and update order status to delivered
// TO DO: Refactor Configuration Files
// TO DO: Refactor Entity for not instance a new ID on Get register on Database and map the Entity with a different ID
// TO DO: Configure Logs

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    _ = options.Filters.Add<ExceptionFilter>();
    _ = options.Filters.Add<NotificationFilter>();
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.MaxDepth = 5;
});

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.UsingRabbitMq((context, config) =>
    {
        config.Host(builder.Configuration.GetConnectionString("RabbitMqConnection"));
        config.ConfigureEndpoints(context);
    });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContextPool<DeliveryPostgresDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DeliveryPostgresDbContext"));
    options.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
});

builder.Services.AddStackExchangeRedisCache(options =>
    options.Configuration = builder.Configuration.GetConnectionString("RedisDbConnection"));

builder.Services.AddHybridCache(options =>
{
    options.MaximumPayloadBytes = 1024 * 1024;
    options.MaximumKeyLength = 1024;
    options.DefaultEntryOptions = new HybridCacheEntryOptions
    {
        Expiration = TimeSpan.FromMinutes(5),
        LocalCacheExpiration = TimeSpan.FromMinutes(5)
    };
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateDeliveryCommand).Assembly));

builder.Services.AddDependencyInjections();

var section = builder.Configuration.GetSection("ExternalApiSettings");
var settings = section.Get<ExternalApiSettings>();

builder.Services.AddHttpClientConfiguration(settings, builder.Environment.IsDevelopment());

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    DbMigrationConfiguration.Configure(app);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
