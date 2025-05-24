using EcommercePortfolio.Orders.API.Application.Commands;
using EcommercePortfolio.Orders.API.Configurations;
using EcommercePortfolio.Orders.Infra.Data;
using EcommercePortfolio.Services.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddApiConfig();

builder.Services.AddMessageBus(builder.Configuration);

builder.Services.AddPostgresDatabase<OrderPostgresDbContext>(
    builder.Configuration, 
    builder.Environment, 
    "OrderPostgresDbConnection");

builder.Services.AddCache(builder.Configuration);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateOrderCommand).Assembly));

builder.Services.AddDependencyInjections();

builder.Services.AddHttpClientConfiguration(builder.Configuration, builder.Environment.IsDevelopment());


var app = builder.Build();

app.MapDefaultEndpoints();

app.UseApiConfiguration(app.Environment);

await app.RunAsync();


namespace EcommercePortfolio.Orders.API
{
    public partial class Program { }
}