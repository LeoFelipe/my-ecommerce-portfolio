using EcommercePortfolio.Deliveries.API.Application.Commands;
using EcommercePortfolio.Deliveries.API.Configurations;
using EcommercePortfolio.Deliveries.Infra.Data;
using EcommercePortfolio.Services.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddApiConfig();

builder.Services.AddMessageBus(builder.Configuration);

builder.Services.AddPostgresDatabase<DeliveryPostgresDbContext>(
    builder.Configuration, 
    "PostgresDb");

builder.Services.AddCache(builder.Configuration);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateDeliveryCommand).Assembly));

builder.Services.AddDependencyInjections();

builder.Services.AddHttpClientConfiguration(builder.Configuration);

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseApiConfiguration(app.Environment);

await app.RunAsync();


namespace EcommercePortfolio.Deliveries.API
{
    public partial class Program { }
}