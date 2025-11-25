using Notiway.Common.Core.Enums;
using Notiway.Common.Core.Models;

namespace Notiway.Common.Plugins.Interfaces;
public interface IStorage<TItem>
{
    Task<Processing> SaveAsync(TItem item, bool skipVerCheck = true, CancellationToken cancellationToken = default);

    Task<Result<TItem>> GetAsync(string partitionKey, string notificationId, CancellationToken cancellationToken = default);

    Task<Results<TItem>> GetAllAsync(string partitionKey, int limit = 1000, string? paginationToken = null, CancellationToken cancellationToken = default);

    Task<Processing> SaveUniqueAsync(TItem item, CancellationToken cancellationToken = default);
}
