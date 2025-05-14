using EcommercePortfolio.Carts.API.Application.Commands;
using EcommercePortfolio.Carts.API.Configurations;
using EcommercePortfolio.Carts.Infra.Data;
using EcommercePortfolio.Services.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddApiConfig();

builder.Services.AddMessageBus(builder.Configuration);

builder.Services.AddMongoDatabase<MongoDbContext>(builder.Configuration, "EcommercePortfolioCart");

builder.Services.AddCache(builder.Configuration);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateCartCommand).Assembly));

builder.Services.AddDependencyInjections();

builder.Services.AddHttpClientConfiguration(builder.Configuration);

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseApiConfiguration(app.Environment);

await app.RunAsync();