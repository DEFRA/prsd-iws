namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using Core.ImportNotificationAssessment;
    using Core.Shared;

    public class ImportNotificationSearchResult
    {
        public Guid NotificationId { get; private set; }

        public string Number { get; private set; }

        public ImportNotificationStatus Status { get; private set; }

        public string Exporter { get; private set; }

        public string Importer { get; private set; }

        public NotificationType NotificationType { get; private set; }

        public ImportNotificationSearchResult(Guid notificationId,
            string number, 
            ImportNotificationStatus status, 
            string exporter, 
            string importer,
            NotificationType notificationType)
        {
            NotificationId = notificationId;
            NotificationType = notificationType;
            Number = number;
            Status = status;
            Exporter = exporter;
            Importer = importer;
        }
    }
}