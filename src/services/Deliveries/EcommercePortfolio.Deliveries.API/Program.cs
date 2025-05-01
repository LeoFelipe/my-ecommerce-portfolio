using EcommercePortfolio.Deliveries.API.Applications.Commands;
using EcommercePortfolio.Deliveries.API.Configurations;
using EcommercePortfolio.Deliveries.Infra.Data;
using EcommercePortfolio.Services.Configurations;

// TO DO: Refactor Entity for not instance a new ID on Get register on Database and map the Entity with a different ID
// TO DO: Configure Logs

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddApiConfig();

builder.Services.AddMessageBus(builder.Configuration);

builder.Services.AddPostgresDatabase<DeliveryPostgresDbContext>(builder.Configuration, "EcommercePortfolioDelivery");

builder.Services.AddCache(builder.Configuration);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateDeliveryCommand).Assembly));

builder.Services.AddDependencyInjections();

builder.Services.AddHttpClientConfiguration(builder.Configuration, builder.Environment.IsDevelopment());

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseApiConfiguration(app.Environment);

await app.RunAsync();