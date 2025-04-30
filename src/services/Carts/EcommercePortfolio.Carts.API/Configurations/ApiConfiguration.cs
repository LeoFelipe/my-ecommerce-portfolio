using EcommercePortfolio.Services.Configurations;
using EcommercePortfolio.Services.Filters;
using Scalar.AspNetCore;

namespace EcommercePortfolio.Carts.API.Configurations;

public static class ApiConfiguration
{
    public static void AddApiConfig(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            _ = options.Filters.Add<ExceptionFilter>();
            _ = options.Filters.Add<NotificationFilter>();
        })
        .AddJsonOptions(options => options.JsonSerializerOptions.Default());

        services.AddCors(options =>
        {
            options.AddPolicy("OpenPolicy", builder =>
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });

        services.AddOpenApi();
    }

    public static void UseApiConfiguration(this WebApplication app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseCors("OpenPolicy");

        app.MapControllers();
    }
}
