using EcommercePortfolio.API;
using EcommercePortfolio.API.Configurations;
using EcommercePortfolio.API.Filters;
using EcommercePortfolio.Infra.Contexts;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    _ = options.Filters.Add<ExceptionFilter>();
    _ = options.Filters.Add<NotificationFilter>();
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContextPool<MongoDbContext>(options =>
    options.UseMongoDB(builder.Configuration.GetConnectionString("MongoDbConnection"), "ecommerce-portfolio"));

builder.Services.AddDbContextPool<PostgresDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresDbContext")));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddDependencyInjections();

var section = builder.Configuration.GetSection("ExternalApiSettings");
var settings = section.Get<ExternalApiSettings>();

builder.Services.AddHttpClientConfiguration(settings);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
