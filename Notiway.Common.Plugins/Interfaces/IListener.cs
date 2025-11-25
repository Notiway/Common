namespace Notiway.Common.Plugins.Interfaces;
public interface IListener
{
    Task ConsumeAsync<TMessage>(Func<TMessage, Task> ProcessMessageAsync, CancellationToken cancellationToken, int retry = 3);
}
