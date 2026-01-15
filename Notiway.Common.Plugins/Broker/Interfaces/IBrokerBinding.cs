namespace Notiway.Common.Plugins.Broker.Interfaces;

public interface IBrokerBinding
{
    string Id { get; }
    Task UnbindAsync();
}
