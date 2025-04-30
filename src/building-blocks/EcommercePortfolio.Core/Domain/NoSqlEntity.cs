using MongoDB.Bson;

namespace EcommercePortfolio.Core.Domain;

public abstract class NoSqlEntity : Entity
{
    public string Id { get; private set; }

    protected NoSqlEntity()
    {
        Id = ObjectId.GenerateNewId().ToString();
    }
}
