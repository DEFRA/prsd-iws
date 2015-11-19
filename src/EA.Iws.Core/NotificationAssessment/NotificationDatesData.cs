namespace EA.Iws.Core.NotificationAssessment
{
    using System;

    public class NotificationDatesData
    {
        public DateTime? NotificationReceivedDate { get; set; }

        public Guid NotificationId { get; set; }

        public DateTime? PaymentReceivedDate { get; set; }

        public bool PaymentIsComplete { get; set; }

        public DateTime? CommencementDate { get; set; }

        public string NameOfOfficer { get; set; }

        public DateTime? CompletedDate { get; set; }

        public DateTime? TransmittedDate { get; set; }

        public DateTime? AcknowledgedDate { get; set; }

        public DateTime? DecisionRequiredDate { get; set; }
    }
}