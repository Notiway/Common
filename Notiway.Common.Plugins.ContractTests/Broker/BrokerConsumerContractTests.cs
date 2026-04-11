using Notiway.Common.Plugins.Broker.Interfaces;

namespace Notiway.Common.Plugins.ContractTests.Broker;

/// <summary>
/// Contract tests for IBrokerConsumer implementations.
/// Inherit from this class in your plugin test project and implement the abstract members
/// to provide the concrete consumer and infrastructure setup/teardown.
/// </summary>
public abstract class BrokerConsumerContractTests : IAsyncLifetime
{
    protected abstract IBrokerConsumer CreateConsumer();

    protected abstract string GenerateUniqueEndpointName();

    protected abstract Dictionary<string, string> GetEndpointMetadata();

    public virtual Task InitializeAsync() => Task.CompletedTask;
    public virtual Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task CreateEndpoint_ShouldReturnSuccessfulEndpoint()
    {
        IBrokerConsumer consumer = CreateConsumer();
        string endpointName = GenerateUniqueEndpointName();

        Result<IMessageEndpoint> result = await consumer.CreateEndpointAsync(
            endpointName, GetEndpointMetadata());

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.False(string.IsNullOrEmpty(result.Value.Name));

        await consumer.DeleteEndpointAsync(endpointName);
    }

    [Fact]
    public async Task CreateEndpoint_Twice_ShouldNotFail()
    {
        IBrokerConsumer consumer = CreateConsumer();
        string endpointName = GenerateUniqueEndpointName();

        Result<IMessageEndpoint> first = await consumer.CreateEndpointAsync(
            endpointName, GetEndpointMetadata());
        Result<IMessageEndpoint> second = await consumer.CreateEndpointAsync(
            endpointName, GetEndpointMetadata());

        Assert.True(first.IsSuccess);
        Assert.True(second.IsSuccess);

        await consumer.DeleteEndpointAsync(endpointName);
    }

    [Fact]
    public async Task EndpointExists_AfterCreate_ShouldReturnSuccess()
    {
        IBrokerConsumer consumer = CreateConsumer();
        string endpointName = GenerateUniqueEndpointName();

        await consumer.CreateEndpointAsync(endpointName, GetEndpointMetadata());

        Processing result = await consumer.EndpointExistsAsync(endpointName);

        Assert.Equal(Processing.Success, result);

        await consumer.DeleteEndpointAsync(endpointName);
    }

    [Fact]
    public async Task EndpointExists_NonExistent_ShouldReturnNotFound()
    {
        IBrokerConsumer consumer = CreateConsumer();
        string endpointName = GenerateUniqueEndpointName();

        Processing result = await consumer.EndpointExistsAsync(endpointName);

        Assert.Equal(Processing.NotFound, result);
    }

    [Fact]
    public async Task DeleteEndpoint_Existing_ShouldReturnSuccess()
    {
        IBrokerConsumer consumer = CreateConsumer();
        string endpointName = GenerateUniqueEndpointName();

        await consumer.CreateEndpointAsync(endpointName, GetEndpointMetadata());

        Processing result = await consumer.DeleteEndpointAsync(endpointName);

        Assert.Equal(Processing.Success, result);
    }

    [Fact]
    public async Task DeleteEndpoint_NonExistent_ShouldNotThrow()
    {
        IBrokerConsumer consumer = CreateConsumer();
        string endpointName = GenerateUniqueEndpointName();

        Processing result = await consumer.DeleteEndpointAsync(endpointName);

        Assert.NotEqual(Processing.Error, result);
    }

    [Fact]
    public async Task ConsumeAsync_WithCancellation_ShouldStopGracefully()
    {
        IBrokerConsumer consumer = CreateConsumer();
        string endpointName = GenerateUniqueEndpointName();

        Result<IMessageEndpoint> endpoint = await consumer.CreateEndpointAsync(
            endpointName, GetEndpointMetadata());
        Assert.True(endpoint.IsSuccess);

        using CancellationTokenSource cts = new(TimeSpan.FromSeconds(3));

        await consumer.ConsumeAsync<string>(
            endpoint.Value.Name,
            _ => Task.CompletedTask,
            3,
            cts.Token);

        await consumer.DeleteEndpointAsync(endpointName);
    }
}
