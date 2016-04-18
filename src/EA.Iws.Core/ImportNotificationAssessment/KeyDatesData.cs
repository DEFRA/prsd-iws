namespace EA.Iws.Core.ImportNotificationAssessment
{
    using System;

    public class KeyDatesData
    {
        public DateTime? NotificationReceived { get; set; }

        public DateTime? PaymentReceived { get; set; }

        public DateTime? AssessmentStarted { get; set; }

        public string NameOfOfficer { get; set; }

        public bool IsPaymentComplete { get; set; }

        public DateTime? NotificationCompletedDate { get; set; }

        public DateTime? AcknowlegedDate { get; set; }

        public DateTime? DecisionRequiredByDate { get; set; }

        public bool IsInterim { get; set; }
    }
}
