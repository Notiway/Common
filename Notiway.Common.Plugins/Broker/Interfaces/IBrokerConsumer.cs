using Notiway.Common.Core.Enums;
using Notiway.Common.Core.Models;

namespace Notiway.Common.Plugins.Broker.Interfaces;
public interface IBrokerConsumer
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    /// <param name="endpointTarget">Can be queueName or queueUrl or anything that identifies queue.
    /// Target should be result of queue creation from IQueueManager</param>
    /// <param name="ProcessMessageAsync"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="retry"></param>
    /// <returns></returns>
    Task ConsumeAsync<TMessage>(string endpointTarget, Func<TMessage, Task> ProcessMessageAsync, int retry = 3, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a message endpoint that can receive messages.
    /// </summary>
    /// <param name="definition">Logical endpoint definition</param>
    /// <param name="retry">Retry attempts</param>
    Task<Result<IMessageEndpoint>> CreateEndpointAsync(
        string endpointName,
        Dictionary<string, string> metadata,
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
