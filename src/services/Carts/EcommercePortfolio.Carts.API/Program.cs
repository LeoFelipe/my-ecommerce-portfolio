using EcommercePortfolio.Carts.API.Application.Commands;
using EcommercePortfolio.Carts.API.Configurations;
using EcommercePortfolio.Carts.Infra.Data;
using EcommercePortfolio.Services.Configurations;

// TO DO: Refactor Entity for not instance a new ID on Get register on Database and map the Entity with a different ID
// TO DO: Configure Logs

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddApiConfig();

builder.Services.AddMessageBus(builder.Configuration);

var conn = builder.Configuration.GetConnectionString("ecommerceportfolio-mongo-db");
builder.Services.AddMongoDatabase<MongoDbContext>(builder.Configuration, "MongoDbConnection", "EcommercePortfolioCart");

builder.Services.AddCache(builder.Configuration);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateCartCommand).Assembly));

builder.Services.AddDependencyInjections();

builder.Services.AddHttpClientConfiguration(builder.Configuration);

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseApiConfiguration(app.Environment);

await app.RunAsync();