namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using Prsd.Core.Domain;

    public class NotificationDates : Entity
    {
        internal NotificationDates()
        {
        }

        public DateTime? NotificationReceivedDate { get; internal set; }

        public DateTime? PaymentReceivedDate { get; internal set; }

        public DateTime? CommencementDate { get; internal set; }

        public DateTime? CompleteDate { get; internal set; }

        public DateTime? TransmittedDate { get; internal set; }

        public DateTime? AcknowledgedDate { get; internal set; }

        public string NameOfOfficer { get; internal set; }

        public DateTime? WithdrawnDate { get; internal set; }

        public DateTime? ConsentWithdrawnDate { get; internal set; }

        public DateTime? ConsentedDate { get; internal set; }

        public string ConsentWithdrawnReasons { get; internal set; }

        public DateTime? ObjectedDate { get; internal set; }

        public string ObjectionReason { get; internal set; }

        public string WithdrawnReason { get; internal set; }

        public DateTime? FileClosedDate { get; internal set; }

        public string ArchiveReference { get; internal set; }
    }
}