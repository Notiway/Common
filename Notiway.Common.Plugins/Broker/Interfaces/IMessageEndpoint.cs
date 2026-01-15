using Notiway.Common.Plugins.Broker.Enums;

namespace Notiway.Common.Plugins.Broker.Interfaces;

public interface IMessageEndpoint
{
    string Address { get; }

    string Name { get; }

    string Id { get; }
    EndpointType Type { get; }
    IReadOnlyDictionary<string, string>? Metadata { get; }
}
