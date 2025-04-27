namespace EcommercePortfolio.Domain.Deliveries.ApiServices;

public interface IOrderApiService
{
    Task<GetCartByClientIdResponse> GetOrderById(Guid id);
}
