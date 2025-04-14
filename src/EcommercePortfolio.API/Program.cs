using EcommercePortfolio.API.Configurations;
using EcommercePortfolio.Infra.Contexts;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDependencyInjections();

var section = builder.Configuration.GetSection("ExternalApiSettings");
var settings = section.Get<ExternalApiSettings>();

builder.Services.AddHttpClientConfiguration(settings);

builder.Services.AddDbContextPool<PostgresDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgresDbContext")));

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
