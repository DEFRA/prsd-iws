namespace EA.Iws.Core.Notification
{
    using System;

    public class NotificationSharedUser
    {
        public Guid Id { get; set; }

        public Guid NotificationId { get;  set; }
        public string UserId { get;  set; }
        public DateTimeOffset DateAdded { get;  set; }
        public string Email { get; set; }
    }
}
