using Notiway.Common.Core.Builders;

using System.Text.Json.Serialization;

namespace Notiway.Common.Core.Dto;

public record NotificationDto
{
    [JsonInclude]
    public string Id { get; private set; }

    [JsonInclude]
    public string Type { get; private set; }

    [JsonInclude]
    public dynamic? Body { get; private set; }

    [JsonInclude]
    public RoutingDto Routing { get; private set; }

    [JsonInclude]
    public MetadataDto Metadata { get; private set; }

    [JsonConstructor]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    protected NotificationDto() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    public NotificationDto(NotificationBuilder builder)
    {
        builder.Build();
        Id = builder.Id;
        Type = builder.Type;
        Body = builder.Body;
        Routing = builder.Routing;
        Metadata = builder.Metadata;
    }
}

