namespace Notiway.Common.Plugins.Interfaces;
public interface IListener
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    /// <param name="queueTarget">Can be queueName or queueUrl or anything that identifies queue. 
    /// Target should be result of queue creation from IQueueManager</param>
    /// <param name="ProcessMessageAsync"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="retry"></param>
    /// <returns></returns>
    Task ConsumeAsync<TMessage>(string queueTarget, Func<TMessage, Task> ProcessMessageAsync, CancellationToken cancellationToken, int retry = 3);
}
