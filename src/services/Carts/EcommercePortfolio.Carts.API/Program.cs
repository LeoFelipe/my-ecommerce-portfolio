using EcommercePortfolio.Carts.API.Application.Commands;
using EcommercePortfolio.Carts.API.Configurations;
using EcommercePortfolio.Carts.Infra.Data;
using EcommercePortfolio.Services.Configurations;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddApiConfig();

builder.Services.AddMessageBus(builder.Configuration);

builder.Services.AddMongoDatabase<MongoDbContext>(
    builder.Configuration, 
    builder.Environment, 
    "MongoDbConnection", 
    "EcommercePortfolioCart");

foreach (var kv in builder.Configuration.AsEnumerable())
{
    Console.WriteLine($"{kv.Key} = {kv.Value}");
}

builder.Services.AddCache(builder.Configuration);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateCartCommand).Assembly));

builder.Services.AddDependencyInjections();

builder.Services.AddHttpClientConfiguration(builder.Configuration);

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseApiConfiguration(app.Environment, builder.Configuration);

await app.RunAsync();


namespace EcommercePortfolio.Carts.API {
    public partial class Program { }
}