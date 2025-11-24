using Notiway.Common.Core.Enums;

namespace Notiway.Common.Core.Dto;
public record RoutingDto
{
    public string? TenantId { get; set; }

    public required string AudienceValue { get; set; }

    public string AudienceId => AudienceType switch
    {
        AudienceType.Global => "global",
        AudienceType.Tenant => $"tenant:{AudienceValue}",
        AudienceType.Group => string.IsNullOrEmpty(TenantId)
                    ? $"group:{AudienceValue}"
                    : $"group:{TenantId}:{AudienceValue}",
        AudienceType.User => $"user:{AudienceValue}",
        _ => AudienceValue
    };
    public required AudienceType AudienceType { get; set; }
}