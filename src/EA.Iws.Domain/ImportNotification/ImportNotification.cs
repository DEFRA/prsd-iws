namespace EA.Iws.Domain.ImportNotification
{
    using Core.Shared;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class ImportNotification : Entity
    {
        public string NotificationNumber { get; set; }

        public NotificationType NotificationType { get; set; }

        protected ImportNotification()
        {
        }

        public ImportNotification(NotificationType notificationType, 
            string notificationNumber)
        {
            Guard.ArgumentNotNullOrEmpty(() => notificationNumber, notificationNumber);
            Guard.ArgumentNotDefaultValue(() => notificationType, notificationType);

            NotificationType = notificationType;
            NotificationNumber = notificationNumber;
        }
    }
}
