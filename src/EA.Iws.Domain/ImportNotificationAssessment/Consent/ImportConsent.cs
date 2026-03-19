namespace EA.Iws.Domain.ImportNotificationAssessment.Consent
{
    using System;
    using Prsd.Core.Domain;

    public class ImportConsent : Entity
    {
        public DateRange ConsentRange { get; private set; }

        public string Conditions { get; private set; }

        public Guid UserId { get; private set; }

        public Guid NotificationId { get; private set; }

        protected ImportConsent()
        {
        }

        public ImportConsent(Guid notificationId, DateRange consentRange, string conditions, Guid userId)
        {
            ConsentRange = consentRange;
            Conditions = conditions;
            UserId = userId;
            NotificationId = notificationId;
        }

        public void UpdateDateRange(DateTime? fromDate, DateTime? toDate)
        {
            if (fromDate != null && toDate != null)
            {
                ConsentRange = new DateRange(fromDate.Value, fromDate.Value);
            }
        }
    }
}
