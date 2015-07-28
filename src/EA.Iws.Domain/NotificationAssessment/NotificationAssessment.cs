namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using Prsd.Core.Domain;

    public class NotificationAssessment : Entity
    {
        public DateTime? NotificationReceivedDate { get; set; }

        public DateTime? PaymentRecievedDate { get; set; }

        public DateTime? CommencementDate { get; set; }

        public DateTime? CompleteDate { get; set; }

        public DateTime? TransmittedDate { get; set; }

        public DateTime? AcknowledgedDate { get; set; }

        public DateTime? DecisionDate { get; set; }

        public DateTime? DecisionMade { get; set; }

        public DateTime? ConsentedFrom { get; set; }

        public DateTime? ConsentedTo { get; set; }

        public string ConditionsOfConsent { get; set; }

        public int DecisionType { get; set; }

        public string NameOfOfficer { get; set; }

        public Guid NotificationApplicationId { get; private set; }

        protected NotificationAssessment()
        {
        }

        public NotificationAssessment(Guid notificationApplicationId)
        {
            this.NotificationApplicationId = notificationApplicationId;
        }
    }
}
