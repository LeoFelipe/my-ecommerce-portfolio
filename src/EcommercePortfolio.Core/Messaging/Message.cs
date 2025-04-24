using System.Text.Json.Serialization;

namespace EcommercePortfolio.Core.Messaging;

public abstract record Message
{
    [JsonIgnore]
    public string MessageType { get; protected set; }

    [JsonIgnore]
    public DateTime Timestamp { get; private set; }

    protected Message()
    {
        MessageType = GetType().Name;
        Timestamp = DateTime.Now;
    }
}
