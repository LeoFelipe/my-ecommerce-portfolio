namespace EcommercePortfolio.Deliveries.Domain.ApiServices;

public interface IOrderApiService
{
    Task<GetOrderByIdResponse> GetOrderById(Guid id);
}
