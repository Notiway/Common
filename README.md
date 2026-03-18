# Notiway Common

Shared libraries for the [Notiway](https://notiway.net) notification gateway ecosystem. This repository contains the core models, enums, and plugin interfaces that all Notiway components depend on.

## Packages

| Package | Description | NuGet |
|---------|-------------|-------|
| **Notiway.Common.Core** | Core models, DTOs, enums, and result types used across the Notiway ecosystem | [![NuGet](https://img.shields.io/nuget/v/Notiway.Common.Core)](https://www.nuget.org/packages/Notiway.Common.Core) |
| **Notiway.Common.Plugins** | Plugin contract interfaces for building custom Notiway plugins | [![NuGet](https://img.shields.io/nuget/v/Notiway.Common.Plugins)](https://www.nuget.org/packages/Notiway.Common.Plugins) |

## Notiway.Common.Core

Shared core library containing the foundational types used throughout the Notiway ecosystem.

### Key types

| Type | Description |
|------|-------------|
| `NotificationDto` | Notification payload with routing, metadata, and body |
| `RoutingDto` | Audience targeting — type, value, and computed audience ID |
| `MetadataDto` | Producer name, timestamp, and persistence settings |
| `AudienceType` | Enum: Global, Tenant, Group, User, Connection |
| `Processing` | Result codes: Success, NotFound, Unauthorized, Forbidden, Error, etc. |
| `Result<T>` / `Results<T>` | Result wrappers with status codes and pagination support |
| `PersistedNotification` | Storage model for offline notification persistence |
| `NotificationUser` | Storage model for per-user read/deleted state |

### Install

```shell
dotnet add package Notiway.Common.Core
```

## Notiway.Common.Plugins

Plugin contract library for building custom plugins that integrate with the Notiway runtime.

### Plugin types

Every plugin implements `IPlugin` which provides service registration, middleware hooks, and provider metadata. Specialized interfaces identify the plugin category:

| Interface | Purpose |
|---|---|
| `IBufferPlugin` | Consume messages from a queue or stream (SQS, Redis Streams, Kafka, etc.) |
| `IBrokerPlugin` | Publish messages across gateway instances (SNS, Redis Pub/Sub, Kafka, etc.) |
| `IStoragePlugin` | Persist and retrieve notifications (DynamoDB, PostgreSQL, MongoDB, etc.) |
| `IAuthPlugin` | Authenticate incoming connections (JWT, etc.) |
| `ITenantValidationPlugin` | Validate tenant access and authorization |
| `IHostPlugin` | Provide host-level runtime configuration |
| `IMiddlewarePlugin` | Register custom ASP.NET middleware in the pipeline |

### Install

```shell
dotnet add package Notiway.Common.Plugins
```

## Building custom plugins

1. Create a new class library targeting **net10.0**
2. Install **Notiway.Common.Plugins**
3. Implement `IPlugin` and the relevant category interface
4. Register your services in `Register()` and middleware in `Use()`

For existing plugin implementations, see the [Notiway GitHub organization](https://github.com/Notiway).

## License

[MIT](LICENSE)

## Links

- [Documentation](https://notiway.net)
- [GitHub](https://github.com/Notiway)
