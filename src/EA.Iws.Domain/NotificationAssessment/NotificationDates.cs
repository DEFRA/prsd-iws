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

        public DateTime? TransmittedDate { get; set; }

        public DateTime? AcknowledgedDate { get; set; }

        public DateTime? DecisionDate { get; set; }

        public string NameOfOfficer { get; internal set; }
    }
}