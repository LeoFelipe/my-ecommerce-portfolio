namespace EcommercePortfolio.Deliveries.Domain.ApiServices;

public interface IOrderApiService
{
    Task<GetAddressOrderByIdResponse> GetAddressOrderById(Guid id);
}
