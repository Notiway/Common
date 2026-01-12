using Notiway.Common.Core.Enums;
using Notiway.Common.Core.Models;

namespace Notiway.Common.Plugins.Interfaces;

public interface IQueueManager
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="queueName">queue name consist of {env}-notiway-gateway-{uniqueServiceId}</param>
    /// <param name="messageRetentionPeriod">how long message will be retained in queue in seconds</param>
    /// <param name="tags">tags to easier identify where this queue belongs</param>
    /// <param name="retry">how many times we will retry this operation in case of failure</param>
    /// <returns></returns>
    Task<Result<string>> ConstructQueueAsync(
            string queueName,
            int messageRetentionPeriod = 3600,
            Dictionary<string, string>? tags = null,
            int retry = 3);

    Task<Processing> IsQueueExistAsync(string queueName, int retry = 3);

    Task<Processing> DeleteQueueAsync(string queueName, int retry = 3);
}
