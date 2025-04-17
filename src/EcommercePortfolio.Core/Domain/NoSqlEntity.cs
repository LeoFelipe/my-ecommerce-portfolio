using MongoDB.Bson;

namespace EcommercePortfolio.Core.Domain;

public abstract class NoSqlEntity
{
    public string Id { get; private set; }

    protected NoSqlEntity()
    {
        Id = ObjectId.GenerateNewId().ToString();
    }
}
