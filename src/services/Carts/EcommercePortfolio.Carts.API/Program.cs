using EcommercePortfolio.Carts.API.Application.Commands;
using EcommercePortfolio.Carts.API.Configurations;
using EcommercePortfolio.Carts.Infra.Data;
using EcommercePortfolio.Services.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddApiConfig();

builder.Services.AddMessageBus(builder.Configuration);

builder.Services.AddMongoDatabase<MongoDbContext>(
    builder.Configuration, 
    "MongoDb", 
    "EcommercePortfolioCart");

var testConnectionString = builder.Configuration.GetConnectionString("MongoDb");
Console.WriteLine($"[DEBUG] MongoDb: {testConnectionString}");

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