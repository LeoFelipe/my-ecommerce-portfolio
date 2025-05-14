namespace EcommercePortfolio.Core.Domain;

public abstract class SqlEntity : Entity
{
    public Guid Id { get; private set; }

    protected SqlEntity()
    {
        if (Id == Guid.Empty)
            Id = Guid.CreateVersion7();
    }
}
