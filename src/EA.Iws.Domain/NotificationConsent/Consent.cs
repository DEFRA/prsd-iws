namespace EA.Iws.Domain.NotificationConsent
{
    using System;
    using Prsd.Core.Domain;

    public class Consent : Entity
    {
        public DateRange ConsentRange { get; set; }

        public string Conditions { get; private set; }

        public Guid UserId { get; private set; }

        public Guid NotificationApplicationId { get; set; }

        protected Consent()
        {
        }

        public Consent(Guid notificationApplicationId, DateRange consentRange, string conditions, Guid userId)
        {
            NotificationApplicationId = notificationApplicationId;

            ConsentRange = consentRange;

            Conditions = conditions;

            UserId = userId;
        }
    }
}
