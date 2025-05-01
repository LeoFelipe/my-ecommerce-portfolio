using EcommercePortfolio.Orders.API.Applications.Commands;
using EcommercePortfolio.Orders.API.Configurations;
using EcommercePortfolio.Orders.Infra.Data;
using EcommercePortfolio.Services.Configurations;

// TO DO: Refactor Entity for not instance a new ID on Get register on Database and map the Entity with a different ID
// TO DO: Configure Logs

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddApiConfig();

builder.Services.AddMessageBus(builder.Configuration);

builder.Services.AddPostgresDatabase<OrderPostgresDbContext>(builder.Configuration, "EcommercePortfolioOrder");

builder.Services.AddCache(builder.Configuration);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateOrderCommand).Assembly));

builder.Services.AddDependencyInjections();

builder.Services.AddHttpClientConfiguration(builder.Configuration, builder.Environment.IsDevelopment());


var app = builder.Build();

app.MapDefaultEndpoints();

app.UseApiConfiguration(app.Environment);

await app.RunAsync();
