using Notiway.Common.Core.Dto;
using Notiway.Common.Core.Enums;

namespace Notiway.Common.Core.Builders;
public class NotificationBuilder
{
    public required string Type { get; init; }

    public dynamic? Body { get; init; }

    /// <summary>
    /// Use only when AudienceType is Group that is Tenant Scoped
    /// </summary>
    public string? TenantId { get; init; }

    public required string Producer { get; init; }

    public bool IsPersisted { get; init; } = false;

    public DateTime? PersistedTTL { get; init; } = DateTime.UtcNow.AddMinutes(30);

    public string AudienceValue { get; init; } = "global";

    public AudienceType AudienceType { get; init; }

    public MetadataDto Metadata { get; private set; } = default!;

    public RoutingDto Routing { get; private set; } = default!;

    public string Id { get; private set; } = default!;

    public void Build()
    {
        Metadata = new()
        {
            Producer = Producer!,
            IsPersisted = IsPersisted,
            PersistedTTL = IsPersisted ? PersistedTTL : null
        };
        Id = $"{Producer}-{Type}-{DateTime.UtcNow:yyyy-MM-dd'T'HH:mm:ss'Z'}";
        if(AudienceValue == null && AudienceType != AudienceType.Global)
        {
            throw new Exception("Audience Value Must Be Populated for any audience Except Global");
        }
        if(AudienceType != AudienceType.Global)
        {
            if(TenantId != null)
            {
                if(AudienceType == AudienceType.Tenant)
                {
                    if(AudienceValue != TenantId)
                    {
                        throw new Exception("AudienceValue must match TenantId for Tenant Audience Type when tenantID is populated");
                    }
                }
            }
        }
        Routing = new()
        {
            AudienceType = AudienceType,
            TenantId = TenantId,
            AudienceValue = AudienceValue!
        };
    }
}
