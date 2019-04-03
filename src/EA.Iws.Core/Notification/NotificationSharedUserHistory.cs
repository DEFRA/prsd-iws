namespace EA.Iws.Core.Notification
{
    using System;

    public class NotificationSharedUserHistory
    {
        public Guid NotificationId { get; set; }
        public Guid UserId { get; set; }
        public DateTimeOffset DateAdded { get; set; }
    }
}
