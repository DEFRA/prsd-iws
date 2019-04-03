namespace EA.Iws.Core.Admin.Reports
{
    using System;
    using System.ComponentModel;
    using ImportNotificationAssessment;
    using Shared;

    public class DataImportNotificationData
    {
        public string NotificationNumber { get; set; }

        [DisplayName("Notification Type (R/D)")]
        public NotificationType NotificationType { get; set; }

        public ImportNotificationStatus Status { get; set; }

        [DisplayName("Pre-consented")]
        public string Preconsented { get; set; }

        public DateTime? ConsentDate { get; set; }

        public DateTime? NotificationReceived { get; set; }

        public DateTime? PaymentReceived { get; set; }

        public DateTime? AssessmentStarted { get; set; }

        public DateTime? ApplicationCompleted { get; set; }

        public DateTime? Acknowledged { get; set; }

        public DateTime? DecisionDate { get; set; }

        public DateTime? DecisionRequiredDate { get; set; }

        [DisplayName("Days to Assessment Started")]
        public int? AssessmentStartedElapsedWorkingDays { get; set; }

        [DisplayName("Received to Acknowledged Days")]
        public int? ReceivedToAcknowledgedElapsedWorkingDays { get; set; }

        [DisplayName("Complete to Acknowledged Days")]
        public int? CompleteToAcknowledgedElapsedWorkingDays { get; set; }

        [DisplayName("Received to Consent Days")]
        public int? ReceivedToConsentElapsedWorkingDays { get; set; }

        [DisplayName("Decision to Consent Days")]
        public int? DecisionToConsentElapsedWorkingDays { get; set; }

        public string Officer { get; set; }

        [DisplayName("Consent valid To date")]
        public DateTime? ConsentTo { get; set; }
    }
}