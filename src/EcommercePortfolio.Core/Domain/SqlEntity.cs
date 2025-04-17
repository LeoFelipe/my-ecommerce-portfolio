namespace EcommercePortfolio.Core.Domain;

public abstract class SqlEntity
{
    public Guid Id { get; private set; }

    protected SqlEntity()
    {
        Id = Guid.CreateVersion7();
    }
}
