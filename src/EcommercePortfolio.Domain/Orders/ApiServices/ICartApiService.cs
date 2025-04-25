namespace EcommercePortfolio.Domain.Orders.ApiServices;

public interface ICartApiService
{
    Task<GetCartByClientIdResponse> GetCartByClientId(Guid clientId);
}
