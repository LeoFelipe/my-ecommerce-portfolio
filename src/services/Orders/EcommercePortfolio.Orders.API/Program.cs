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
    "PostgresDb");

var testConnectionString = builder.Configuration.GetConnectionString("PostgresDb");
Console.WriteLine($"[DEBUG] PostgresDb: {testConnectionString}");

builder.Services.AddCache(builder.Configuration);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateOrderCommand).Assembly));

builder.Services.AddDependencyInjections();

builder.Services.AddHttpClientConfiguration(builder.Configuration);


var app = builder.Build();

app.MapDefaultEndpoints();

app.UseApiConfiguration(app.Environment);

await app.RunAsync();


namespace EcommercePortfolio.Orders.API
{
    public partial class Program { }
}