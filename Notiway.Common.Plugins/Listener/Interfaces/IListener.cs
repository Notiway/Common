namespace Notiway.Common.Plugins.Listener.Interfaces;
public interface IListener
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
}
