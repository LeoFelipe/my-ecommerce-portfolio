namespace EcommercePortfolio.Orders.Domain.ApiServices;

public interface ICartApiService
{
    Task<GetCartByClientIdResponse> GetCartByClientId(Guid clientId);
}
