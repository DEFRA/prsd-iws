namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using Prsd.Core.Domain;

    public class NotificationDecision : Entity
    {
        protected NotificationDecision()
        {
        }

        public NotificationDecision(Guid notificationApplicationId)
        {
            NotificationApplicationId = notificationApplicationId;
        }

        public Guid NotificationApplicationId { get; private set; }

        public DateTime? DecisionMade { get; set; }

        public DateTime? ConsentedFrom { get; set; }

        public DateTime? ConsentedTo { get; set; }

        public string ConditionsOfConsent { get; set; }

        public int DecisionType { get; set; }
    }
}