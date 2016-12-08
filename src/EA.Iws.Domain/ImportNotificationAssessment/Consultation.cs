namespace EA.Iws.Domain.ImportNotificationAssessment
{
    using System;
    using Prsd.Core.Domain;

    public class Consultation : Entity
    {
        public Guid NotificationId { get; private set; }

        public Guid? LocalAreaId { get; set; }

        protected Consultation()
        {
        }

        public Consultation(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}