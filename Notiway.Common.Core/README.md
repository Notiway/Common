# Notiway.Common.Core

Shared core library for the [Notiway](https://notiway.net) notification gateway ecosystem.

> **Note:** This package is published publicly because the [Notiway plugin repositories](https://github.com/Notiway) are open-source and depend on it at build time. It is **not** intended for direct consumption outside of the Notiway ecosystem.

## What's inside

| Namespace | Contents |
|---|---|
| `Enums` | `AudienceType`, `Processing`, `InfrastructureProviders` |
| `Dto` | `NotificationDto`, `RoutingDto`, `MetadataDto` |
| `Models` | `Result<T>`, `Results<T>`, `PersistedNotification`, `NotificationUser` |
| `Builders` | `NotificationBuilder` |
| `Constants` | `LoggingCodes` |

## Installation

```shell
dotnet add package Notiway.Common.Core
```

## License

[MIT](https://opensource.org/licenses/MIT)

## Links

- [Documentation](https://notiway.net)
- [Source](https://github.com/Notiway/Common)
