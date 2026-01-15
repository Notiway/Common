using Notiway.Common.Plugins.Broker.Enums;

namespace Notiway.Common.Plugins.Broker.Interfaces;

public interface IMessageEndpoint
{
    string Address { get; }
    EndpointType Type { get; }
    IReadOnlyDictionary<string, string>? Metadata { get; }
}
