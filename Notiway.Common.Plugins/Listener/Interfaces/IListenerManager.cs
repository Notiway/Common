using Notiway.Common.Core.Enums;
using Notiway.Common.Core.Models;
using Notiway.Common.Plugins.Broker.Interfaces;

namespace Notiway.Common.Plugins.Listener.Interfaces;

public interface IListenerManager
{
    /// <summary>
    /// Creates a message endpoint that can receive messages.
    /// </summary>
    /// <param name="definition">Logical endpoint definition</param>
    /// <param name="retry">Retry attempts</param>
    Task<Result<IMessageEndpoint>> CreateEndpointAsync(
        string endpointName,
        Dictionary<string, string>? metadata = null,
        int retry = 3,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks whether an endpoint exists.
    /// </summary>
    Task<Processing> EndpointExistsAsync(
        string endpointName,
        int retry = 3,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an endpoint.
    /// </summary>
    Task<Processing> DeleteEndpointAsync(
        string endpointName,
        int retry = 3,
        CancellationToken cancellationToken = default);
}