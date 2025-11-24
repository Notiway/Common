namespace Notiway.Common.Core.Models;

/// <summary>
/// Some notifications we want to persist accross sessions. 
/// For example we want to notify users about change that happened in past when they werent connected to gateway
/// 
/// PartitionKey = GroupId
/// </summary>
public class PersistedNotification
{
    public required string PartitionKey { get; set; }

    public required string NotificationId { get; set; }

    /// <summary>
    /// Type of notification
    /// </summary>
    public required string Type { get; set; }

    /// <summary>
    /// Optional
    /// TenantId of notification
    /// </summary>
    public string? TenantId { get; set; }

    /// <summary>
    /// Time to live
    /// After that time notification will not be available
    /// </summary>
    public long TimeToLive { get; set; }

    /// <summary>
    /// Content of notification
    /// </summary>
    public required string Message { get; set; }
}
