namespace Notiway.Common.Core.Dto;
public record MetadataDto
{
    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;

    public required string Producer { get; set; }

    public bool IsPersisted { get; set; } = false;

    public DateTime? PersistedTTL { get; set; }
}

