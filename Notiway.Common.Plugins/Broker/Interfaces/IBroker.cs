using Notiway.Common.Core.Enums;
using Notiway.Common.Core.Models;

namespace Notiway.Common.Plugins.Broker.Interfaces;
public interface IBroker
{
    Task<Processing> SendNotificationAsync<T>(T notification);

    string GetBrokerAddress();

    Task<Result<IBrokerBinding>> BindAsync(IMessageEndpoint endpoint);
}
