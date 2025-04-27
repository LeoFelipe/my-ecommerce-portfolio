using EcommercePortfolio.Domain.Deliveries.Entities;

namespace EcommercePortfolio.Domain.Deliveries;

public interface IDeliveryRepository : IDisposable
{
    Task<Delivery> GetById(Guid id);
    Task<IEnumerable<Delivery>> GetByClientId(Guid clientId);
    Task AddAsync(Delivery delivery);
    void Update(Delivery delivery);
}