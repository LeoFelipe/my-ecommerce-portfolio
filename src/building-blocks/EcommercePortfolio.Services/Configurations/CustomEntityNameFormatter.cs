using MassTransit;

namespace EcommercePortfolio.Services.Configurations;

public class CustomEntityNameFormatter : IEntityNameFormatter
{
    public string FormatEntityName<T>()
    {
        return typeof(T).Name;
    }
}
