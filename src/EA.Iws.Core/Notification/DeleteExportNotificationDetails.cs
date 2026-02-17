namespace EA.Iws.Core.Notification
{
    using System;

    public class DeleteExportNotificationDetails
    {
        public Guid NotificationId { get; set; }

        public bool IsNotificationCanDeleted { get; set; }

        public string ErrorMessage { get; set; }
    }
}
