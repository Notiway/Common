using Notiway.Common.Plugins.Storage.Interfaces;

namespace Notiway.Common.Plugins.ContractTests.Storage;

/// <summary>
/// Contract tests for IStorage implementations.
/// Inherit from this class in your plugin test project and provide
/// the concrete storage instance and test item factory.
/// </summary>
public abstract class StorageContractTests<TItem> : IAsyncLifetime
{
    protected abstract IStorage<TItem> CreateStorage();

    /// <summary>
    /// Create a test item with the given partition key and notification ID.
    /// </summary>
    protected abstract TItem CreateTestItem(string partitionKey, string notificationId);

    /// <summary>
    /// Extract the partition key from an item (for assertion purposes).
    /// </summary>
    protected abstract string GetPartitionKey(TItem item);

    /// <summary>
    /// Extract the notification ID from an item (for assertion purposes).
    /// </summary>
    protected abstract string GetNotificationId(TItem item);

    public virtual Task InitializeAsync() => Task.CompletedTask;
    public virtual Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task SaveAndGet_ShouldRoundTrip()
    {
        IStorage<TItem> storage = CreateStorage();
        string pk = $"test-{Guid.NewGuid()}";
        string nid = Guid.NewGuid().ToString();
        TItem item = CreateTestItem(pk, nid);

        Processing saveResult = await storage.SaveAsync(item);
        Assert.Equal(Processing.Success, saveResult);

        Result<TItem> getResult = await storage.GetAsync(pk, nid);
        Assert.True(getResult.IsSuccess);
        Assert.Equal(pk, GetPartitionKey(getResult.Value));
        Assert.Equal(nid, GetNotificationId(getResult.Value));
    }

    [Fact]
    public async Task Get_NonExistent_ShouldReturnFailure()
    {
        IStorage<TItem> storage = CreateStorage();

        Result<TItem> result = await storage.GetAsync(
            $"nonexistent-{Guid.NewGuid()}", Guid.NewGuid().ToString());

        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task GetAll_ShouldReturnSavedItems()
    {
        IStorage<TItem> storage = CreateStorage();
        string pk = $"test-{Guid.NewGuid()}";
        int count = 3;

        for(int i = 0; i < count; i++)
        {
            TItem item = CreateTestItem(pk, Guid.NewGuid().ToString());
            await storage.SaveAsync(item);
        }

        Results<TItem> results = await storage.GetAllAsync(pk);

        Assert.True(results.IsSuccess);
        Assert.Equal(count, results.Count);
    }

    [Fact]
    public async Task GetAll_EmptyPartition_ShouldReturnEmptyCollection()
    {
        IStorage<TItem> storage = CreateStorage();

        Results<TItem> results = await storage.GetAllAsync($"empty-{Guid.NewGuid()}");

        Assert.True(results.IsSuccess);
        Assert.Equal(0, results.Count);
    }

    [Fact]
    public async Task GetAll_WithLimit_ShouldRespectLimit()
    {
        IStorage<TItem> storage = CreateStorage();
        string pk = $"test-{Guid.NewGuid()}";

        for(int i = 0; i < 5; i++)
        {
            await storage.SaveAsync(CreateTestItem(pk, Guid.NewGuid().ToString()));
        }

        Results<TItem> results = await storage.GetAllAsync(pk, limit: 2);

        Assert.True(results.IsSuccess);
        Assert.True(results.Count <= 2);
    }

    [Fact]
    public async Task SaveUniqueAsync_DuplicateKey_ShouldReturnAlreadyExists()
    {
        IStorage<TItem> storage = CreateStorage();
        string pk = $"test-{Guid.NewGuid()}";
        string nid = Guid.NewGuid().ToString();

        TItem first = CreateTestItem(pk, nid);
        TItem duplicate = CreateTestItem(pk, nid);

        Processing firstResult = await storage.SaveUniqueAsync(first);
        Assert.Equal(Processing.Success, firstResult);

        Processing duplicateResult = await storage.SaveUniqueAsync(duplicate);
        Assert.Equal(Processing.AlreadyExists, duplicateResult);
    }

    [Fact]
    public async Task SaveAsync_Overwrite_ShouldSucceed()
    {
        IStorage<TItem> storage = CreateStorage();
        string pk = $"test-{Guid.NewGuid()}";
        string nid = Guid.NewGuid().ToString();

        TItem original = CreateTestItem(pk, nid);
        TItem updated = CreateTestItem(pk, nid);

        await storage.SaveAsync(original);
        Processing result = await storage.SaveAsync(updated);

        Assert.Equal(Processing.Success, result);
    }
}
