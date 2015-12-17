namespace EA.Iws.Domain.ImportNotificationAssessment
{
    using System;
    using Prsd.Core.Domain;

    public class ImportNotificationDates : Entity
    {
        public DateTimeOffset? NotificationReceivedDate { get; internal set; }

        public DateTimeOffset? PaymentReceivedDate { get; internal set; }

        internal ImportNotificationDates()
        {
        }
    }
}
