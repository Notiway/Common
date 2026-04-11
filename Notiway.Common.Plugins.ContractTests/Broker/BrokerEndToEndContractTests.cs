using Notiway.Common.Plugins.Broker.Interfaces;

namespace Notiway.Common.Plugins.ContractTests.Broker;

/// <summary>
/// End-to-end contract tests that verify produce → consume round-trip.
/// Inherit from this class to test the full broker pipeline for your plugin.
/// </summary>
public abstract class BrokerEndToEndContractTests : IAsyncLifetime
{
    protected abstract IBrokerProducer CreateProducer();

    protected abstract IBrokerConsumer CreateConsumer();

    protected abstract string GenerateUniqueEndpointName();

    protected abstract Dictionary<string, string> GetEndpointMetadata();

    /// <summary>
    /// Publish a message to the broker so it arrives at the given endpoint.
    /// This is plugin-specific because routing varies per broker (SNS topic, RabbitMQ exchange, etc.)
    /// </summary>
    protected abstract Task PublishMessageToEndpointAsync<T>(T message, IMessageEndpoint endpoint);

    public virtual Task InitializeAsync() => Task.CompletedTask;
    public virtual Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task ProduceAndConsume_MessageShouldBeReceived()
    {
        IBrokerProducer producer = CreateProducer();
        IBrokerConsumer consumer = CreateConsumer();
        string endpointName = GenerateUniqueEndpointName();

        Result<IMessageEndpoint> endpoint = await consumer.CreateEndpointAsync(
            endpointName, GetEndpointMetadata());
        Assert.True(endpoint.IsSuccess);

        Result<IBrokerBinding> bindResult = await producer.BindAsync(endpoint.Value);
        Assert.True(bindResult.IsSuccess);

        TestMessage sent = new() { Id = Guid.NewGuid().ToString(), Content = "contract-test" };
        TestMessage? received = null;
        TaskCompletionSource<TestMessage> tcs = new();

        await PublishMessageToEndpointAsync(sent, endpoint.Value);

        using CancellationTokenSource cts = new(TimeSpan.FromSeconds(15));
        cts.Token.Register(() => tcs.TrySetCanceled());

        Task consumeTask = consumer.ConsumeAsync<TestMessage>(
            endpoint.Value.Name,
            msg =>
            {
                received = msg;
                tcs.TrySetResult(msg);
                return Task.CompletedTask;
            },
            3,
            cts.Token);

        TestMessage result = await tcs.Task;

        Assert.NotNull(result);
        Assert.Equal(sent.Id, result.Id);
        Assert.Equal(sent.Content, result.Content);

        await bindResult.Value.UnbindAsync();
        await consumer.DeleteEndpointAsync(endpointName);
    }

    [Fact]
    public async Task ProduceAndConsume_MultipleMessages_AllShouldBeReceived()
    {
        IBrokerProducer producer = CreateProducer();
        IBrokerConsumer consumer = CreateConsumer();
        string endpointName = GenerateUniqueEndpointName();

        Result<IMessageEndpoint> endpoint = await consumer.CreateEndpointAsync(
            endpointName, GetEndpointMetadata());
        Assert.True(endpoint.IsSuccess);

        Result<IBrokerBinding> bindResult = await producer.BindAsync(endpoint.Value);
        Assert.True(bindResult.IsSuccess);

        int messageCount = 5;
        List<TestMessage> sent = [];
        List<TestMessage> received = [];
        TaskCompletionSource allReceived = new();

        for(int i = 0; i < messageCount; i++)
        {
            TestMessage msg = new() { Id = Guid.NewGuid().ToString(), Content = $"message-{i}" };
            sent.Add(msg);
            await PublishMessageToEndpointAsync(msg, endpoint.Value);
        }

        using CancellationTokenSource cts = new(TimeSpan.FromSeconds(30));
        cts.Token.Register(() => allReceived.TrySetCanceled());

        Task consumeTask = consumer.ConsumeAsync<TestMessage>(
            endpoint.Value.Name,
            msg =>
            {
                received.Add(msg);
                if(received.Count >= messageCount)
                {
                    allReceived.TrySetResult();
                }
                return Task.CompletedTask;
            },
            3,
            cts.Token);

        await allReceived.Task;

        Assert.Equal(messageCount, received.Count);
        foreach(TestMessage sentMsg in sent)
        {
            Assert.Contains(received, r => r.Id == sentMsg.Id);
        }

        await bindResult.Value.UnbindAsync();
        await consumer.DeleteEndpointAsync(endpointName);
    }
}

public class TestMessage
{
    public string Id { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}
