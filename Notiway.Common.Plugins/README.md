# Notiway.Common.Plugins

Plugin contract library for the [Notiway](https://notiway.net) notification gateway. Install this package to build custom plugins that integrate with the Notiway runtime.

## Installation

```shell
dotnet add package Notiway.Common.Plugins
```

## Plugin types

Every plugin implements **IPlugin** which provides service registration, middleware registration, and provider metadata. Specialized marker interfaces identify the plugin category:

| Interface | Purpose |
|---|---|
| **IBufferPlugin** | Consume messages from a queue or stream (SQS, Redis Streams, Kafka, etc.) |
| **IBrokerPlugin** | Publish messages to other services (SNS, Redis Pub/Sub, Kafka, etc.) |
| **IStoragePlugin** | Persist and retrieve notifications (DynamoDB, PostgreSQL, MongoDB, etc.) |
| **IAuthPlugin** | Authenticate incoming connections (JWT, etc.) |
| **ITenantValidationPlugin** | Validate tenant access and authorization |
| **IHostPlugin** | Provide host-level runtime configuration |
| **IMiddlewarePlugin** | Register custom ASP.NET middleware in the pipeline |

## Key interfaces

### IPlugin

Base interface all plugins must implement.

```csharp
public interface IPlugin
{
    string Name { get; }
    int Priority { get; }
    InfrastructureProviders Provider { get; }

    void Register(IServiceCollection services, IConfiguration configuration);
    void Use(IApplicationBuilder app);
}
```

### IBuffer / IBufferManager

Consume messages and manage endpoints (listener plugins).

```csharp
Task ConsumeAsync<TMessage>(string endpointTarget, Func<TMessage, Task> processMessage, ...);
Task<Result<IMessageEndpoint>> CreateEndpointAsync(string endpointName, ...);
Task<Processing> DeleteEndpointAsync(string endpointName, ...);
```

### IBroker

Publish notifications and bind to message endpoints (broker plugins).

```csharp
Task<Processing> SendNotificationAsync<T>(T notification, ...);
Task<Result<IBrokerBinding>> BindAsync(IMessageEndpoint endpoint, ...);
string GetBrokerAddress();
```

### IStorage\<TItem\>

Persist and query items (storage plugins).

```csharp
Task<Processing> SaveAsync(TItem item, ...);
Task<Result<TItem>> GetAsync(string partitionKey, string notificationId, ...);
Task<Results<TItem>> GetAllAsync(string partitionKey, ...);
```

### ITenantValidation

Validate tenant access (tenant validation plugins).

```csharp
Task<Processing> ValidateAsync(string tenantId, string userId, IEnumerable<Claim> claims);
```

## Getting started

1. Create a new class library targeting **net10.0**
2. Install **Notiway.Common.Plugins**
3. Implement **IPlugin** and the relevant category interface
4. Register your services in **Register()** and middleware in **Use()**
5. Package as a NuGet and reference it in your Notiway deployment

For existing plugin implementations, see the [Notiway GitHub organization](https://github.com/Notiway).

## License

[MIT](https://opensource.org/licenses/MIT)

## Links

- [Documentation](https://notiway.net)
- [Source](https://github.com/Notiway/Common)
