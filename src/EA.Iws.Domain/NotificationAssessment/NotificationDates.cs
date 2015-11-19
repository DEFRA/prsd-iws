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

        public DateTime? AcknowledgedDate { get; set; }

        public string NameOfOfficer { get; internal set; }

        public DateTime? WithdrawnDate { get; internal set; }

        public DateTime? ConsentWithdrawnDate { get; set; }

        public string ConsentWithdrawnReasons { get; set; }

        public DateTime? ObjectedDate { get; set; }
    }
}