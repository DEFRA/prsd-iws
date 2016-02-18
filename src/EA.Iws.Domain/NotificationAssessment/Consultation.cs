namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using Prsd.Core.Domain;

    public class Consultation : Entity
    {
        public Guid NotificationId { get; private set; }

        public Guid? LocalAreaId { get; set; }

        public DateTime? ReceivedDate { get; set; }

        protected Consultation()
        {
        }

        public Consultation(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}