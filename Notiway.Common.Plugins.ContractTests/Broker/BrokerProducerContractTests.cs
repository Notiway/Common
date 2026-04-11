using Notiway.Common.Plugins.Broker.Interfaces;

namespace Notiway.Common.Plugins.ContractTests.Broker;

/// <summary>
/// Contract tests for IBrokerProducer implementations.
/// Inherit from this class in your plugin test project and implement the abstract members.
/// </summary>
public abstract class BrokerProducerContractTests : IAsyncLifetime
{
    protected abstract IBrokerProducer CreateProducer();

    protected abstract IBrokerConsumer CreateConsumer();

    protected abstract string GenerateUniqueEndpointName();

    protected abstract Dictionary<string, string> GetEndpointMetadata();

    public virtual Task InitializeAsync() => Task.CompletedTask;
    public virtual Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public void GetBrokerAddress_ShouldReturnNonEmptyString()
    {
        IBrokerProducer producer = CreateProducer();

        string address = producer.GetBrokerAddress();

        Assert.False(string.IsNullOrEmpty(address));
    }

    [Fact]
    public async Task BindAsync_ShouldReturnSuccessfulBinding()
    {
        IBrokerProducer producer = CreateProducer();
        IBrokerConsumer consumer = CreateConsumer();
        string endpointName = GenerateUniqueEndpointName();

        Result<IMessageEndpoint> endpoint = await consumer.CreateEndpointAsync(
            endpointName, GetEndpointMetadata());
        Assert.True(endpoint.IsSuccess);

        Result<IBrokerBinding> bindResult = await producer.BindAsync(endpoint.Value);

        Assert.True(bindResult.IsSuccess);
        Assert.NotNull(bindResult.Value);
        Assert.False(string.IsNullOrEmpty(bindResult.Value.Id));

        await bindResult.Value.UnbindAsync();
        await consumer.DeleteEndpointAsync(endpointName);
    }

    [Fact]
    public async Task UnbindAsync_ShouldCompleteWithoutError()
    {
        IBrokerProducer producer = CreateProducer();
        IBrokerConsumer consumer = CreateConsumer();
        string endpointName = GenerateUniqueEndpointName();

        Result<IMessageEndpoint> endpoint = await consumer.CreateEndpointAsync(
            endpointName, GetEndpointMetadata());
        Result<IBrokerBinding> bindResult = await producer.BindAsync(endpoint.Value);
        Assert.True(bindResult.IsSuccess);

        await bindResult.Value.UnbindAsync();

        await consumer.DeleteEndpointAsync(endpointName);
    }

    [Fact]
    public async Task SendNotificationAsync_ShouldReturnSuccess()
    {
        IBrokerProducer producer = CreateProducer();

        Processing result = await producer.SendNotificationAsync(new { Message = "test" });

        Assert.Equal(Processing.Success, result);
    }
}
