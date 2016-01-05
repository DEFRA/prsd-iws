namespace EA.Iws.Domain.ImportNotificationAssessment
{
    using System;
    using Prsd.Core.Domain;

    public class ImportNotificationDates : Entity
    {
        public DateTimeOffset? NotificationReceivedDate { get; internal set; }

        public DateTimeOffset? PaymentReceivedDate { get; internal set; }

        public DateTimeOffset? AssessmentStartedDate { get; internal set; }

        public string NameOfOfficer { get; internal set; }

        public DateTimeOffset? NotificationCompletedDate { get; internal set; }

        public DateTimeOffset? AcknowledgedDate { get; internal set; }

        public DateTimeOffset? ConsentedDate { get; internal set; }

        internal ImportNotificationDates()
        {
        }
    }
}
