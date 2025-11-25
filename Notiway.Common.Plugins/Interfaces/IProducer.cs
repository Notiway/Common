using Notiway.Common.Core.Enums;

namespace Notiway.Common.Plugins.Interfaces;
public interface IProducer
{
    Task<Processing> SendNotificationAsync<T>(T notification);
}
