using EcommercePortfolio.Core.Data;
using EcommercePortfolio.Deliveries.Domain.Entities;

namespace EcommercePortfolio.Deliveries.Domain;

public interface IDeliveryRepository : IRepository<Delivery>
{
    Task<Delivery> GetById(Guid id);
    Task<Delivery> GetByOrderId(Guid orderId);
    Task AddAsync(Delivery delivery);
    void Update(Delivery delivery);
}