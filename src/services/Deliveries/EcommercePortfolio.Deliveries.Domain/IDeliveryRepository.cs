using EcommercePortfolio.Deliveries.Domain.Entities;

namespace EcommercePortfolio.Deliveries.Domain;

public interface IDeliveryRepository : IDisposable
{
    Task<Delivery> GetById(Guid id);
    Task<IEnumerable<Delivery>> GetByClientId(Guid clientId);
    Task AddAsync(Delivery delivery);
    void Update(Delivery delivery);
}