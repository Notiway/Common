namespace Notiway.Common.Core.Models;

/// <summary>
/// We want to persist information about user interaction with notification
/// PartitionId = UserId
/// </summary>
public class NotificationUser
{
    public required string PartitionKey { get; set; }

    public required string NotificationId { get; set; }
    /// <summary>
    /// Time to live
    /// After that time notification will not be available
    /// </summary>
    public long TimeToLive { get; set; }

    /// <summary>
    /// Have user marked notification as Read?
    /// </summary>
    public bool IsRead { get; set; }

    /// <summary>
    /// Have user marked notification as Deleted?
    /// </summary>
    public bool IsDeleted { get; set; }
}
