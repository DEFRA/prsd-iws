namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using Prsd.Core.Domain;

    public class NotificationDates : Entity
    {
        internal NotificationDates()
        {
        }

        public DateTime? NotificationReceivedDate { get; set; }

        public DateTime? PaymentReceivedDate { get; set; }

        public DateTime? CommencementDate { get; set; }

        public DateTime? CompleteDate { get; set; }

        public DateTime? TransmittedDate { get; set; }

        public DateTime? AcknowledgedDate { get; set; }

        public DateTime? DecisionDate { get; set; }

        public string NameOfOfficer { get; set; }
    }
}