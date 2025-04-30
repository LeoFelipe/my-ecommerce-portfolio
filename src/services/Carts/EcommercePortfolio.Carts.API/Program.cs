using EcommercePortfolio.Carts.API.Application.Commands;
using EcommercePortfolio.Carts.API.Configurations;

// TO DO: Refactor Entity for not instance a new ID on Get register on Database and map the Entity with a different ID
// TO DO: Create Aspire Project
// TO DO: Configure Logs

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddApiConfig();

builder.Services.AddMessageBus(builder.Configuration);

builder.Services.AddDatabases(builder.Configuration);

builder.Services.AddCache(builder.Configuration);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateCartCommand).Assembly));

builder.Services.AddDependencyInjections();

builder.Services.AddHttpClientConfiguration(builder.Configuration);

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseApiConfiguration(app.Environment);

await app.RunAsync();