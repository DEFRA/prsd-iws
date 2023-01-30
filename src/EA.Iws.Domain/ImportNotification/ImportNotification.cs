namespace EA.Iws.Domain.ImportNotification
{
    using Core.Notification;
    using Core.Shared;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using System;

    public class ImportNotification : Entity
    {
        public string NotificationNumber { get; private set; }

        public NotificationType NotificationType { get; private set; }

        public UKCompetentAuthority CompetentAuthority { get; private set; }

        public bool IsArchived { get; set; }

        public DateTime? ArchivedDate { get; set; }
        
        public string ArchivedByUserId { get; set; }

        protected ImportNotification()
        {
        }

        public ImportNotification(NotificationType notificationType,
            UKCompetentAuthority competentAuthority,
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