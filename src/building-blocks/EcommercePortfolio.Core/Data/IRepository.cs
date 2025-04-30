using EcommercePortfolio.Core.Domain;

namespace EcommercePortfolio.Core.Data;

public interface IRepository<T> : IDisposable where T : IAggregateRoot
{
    IUnitOfWork UnitOfWork { get; }
}
