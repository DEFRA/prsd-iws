namespace EA.Iws.Domain.ImportNotification
{
    using Core.Shared;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using CompetentAuthorityEnum = Core.Notification.UKCompetentAuthority;

    public class ImportNotification : Entity
    {
        public string NotificationNumber { get; private set; }

        public NotificationType NotificationType { get; private set; }

        public CompetentAuthorityEnum CompetentAuthority { get; private set; }

        protected ImportNotification()
        {
        }

        public ImportNotification(NotificationType notificationType,
            CompetentAuthorityEnum competentAuthority, 
            string notificationNumber)
        {
            Guard.ArgumentNotNullOrEmpty(() => notificationNumber, notificationNumber);
            Guard.ArgumentNotDefaultValue(() => competentAuthority, competentAuthority);
            Guard.ArgumentNotDefaultValue(() => notificationType, notificationType);

            NotificationType = notificationType;
            CompetentAuthority = competentAuthority;
            NotificationNumber = notificationNumber;
            RaiseEvent(new ImportNotificationCreatedEvent(this));
        }
    }
}
