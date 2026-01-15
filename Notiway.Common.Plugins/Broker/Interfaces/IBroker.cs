using Notiway.Common.Core.Enums;

namespace Notiway.Common.Plugins.Broker.Interfaces;
public interface IBroker
{
    Task<Processing> SendNotificationAsync<T>(T notification);

    string GetBrokerAddress();

    Task<IBrokerBinding> BindAsync(IMessageEndpoint endpoint);
}
