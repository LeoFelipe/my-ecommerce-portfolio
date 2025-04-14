namespace EcommercePortfolio.Core.Data;

public interface IUnitOfWork
{
    Task<bool> Commit();
}
