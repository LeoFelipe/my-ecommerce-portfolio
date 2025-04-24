using FluentValidation.Results;
using MediatR;
using System.Text.Json.Serialization;

namespace EcommercePortfolio.Core.Messaging;

public abstract record Command : Message, IRequest
{
    [JsonIgnore]
    public ValidationResult ValidationResult { get; set; }

    public virtual bool IsValid()
    {
        throw new NotImplementedException();
    }
}
