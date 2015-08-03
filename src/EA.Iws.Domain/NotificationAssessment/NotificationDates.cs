namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using Prsd.Core.Domain;

    public class NotificationDates : Entity
    {
        protected NotificationDates()
        {
        }

        public NotificationDates(Guid notificationApplicationId)
        {
            NotificationApplicationId = notificationApplicationId;
        }

        public Guid NotificationApplicationId { get; private set; }

        public DateTime? NotificationReceivedDate { get; set; }

        public DateTime? PaymentReceivedDate { get; set; }

        public DateTime? CommencementDate { get; set; }

        public DateTime? CompleteDate { get; set; }

        public DateTime? TransmittedDate { get; set; }

        public DateTime? AcknowledgedDate { get; set; }

        public DateTime? DecisionDate { get; set; }

        public string NameOfOfficer { get; set; }
    }
}