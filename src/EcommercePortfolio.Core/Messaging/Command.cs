using FluentValidation.Results;
using MediatR;
using System.Text.Json.Serialization;

namespace EcommercePortfolio.Core.Messaging;

public abstract record Command : IRequest
{
    [JsonIgnore]
    public DateTime Timestamp { get; private set; }

    [JsonIgnore]
    public ValidationResult ValidationResult { get; set; }

    protected Command()
    {
        Timestamp = DateTime.Now;
    }

    public virtual bool IsValid()
    {
        throw new NotImplementedException();
    }
}
