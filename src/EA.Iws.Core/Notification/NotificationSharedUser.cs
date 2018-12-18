namespace EA.Iws.Core.Notification
{
    using System;
    [Serializable]
    public class NotificationSharedUser
    {
        public Guid NotificationId { get;  set; }
        public Guid UserId { get;  set; }
        public string Email { get; set; }
        public DateTimeOffset DateAdded { get;  set; }
    }
}
