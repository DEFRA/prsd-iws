namespace EA.Iws.Domain.ImportNotificationAssessment
{
    using System;
    using Prsd.Core.Domain;

    public class ImportNotificationDates : Entity
    {
        public DateTimeOffset? NotificationReceivedDate { get; private set; }

        public DateTimeOffset? PaymentReceivedDate { get; private set; }

        internal ImportNotificationDates()
        {
        }
    }
}
