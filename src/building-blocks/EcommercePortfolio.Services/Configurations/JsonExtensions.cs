using System.Text.Json.Serialization;
using System.Text.Json;

namespace EcommercePortfolio.Services.Configurations;

public static class JsonExtensions
{
    public static JsonSerializerOptions Default(this JsonSerializerOptions options)
    {
        options.PropertyNameCaseInsensitive = true;
        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.MaxDepth = 5;
        options.Converters.Add(new JsonStringEnumConverter());

        return options;
    }
}
