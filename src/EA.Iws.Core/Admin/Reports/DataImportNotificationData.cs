namespace EA.Iws.Core.Admin.Reports
{
    using System;
    using ImportNotificationAssessment;
    using Shared;

    public class DataImportNotificationData
    {
        public string NotificationNumber { get; set; }

        public NotificationType NotificationType { get; set; }

        public ImportNotificationStatus Status { get; set; }

        public DateTime? ConsentDate { get; set; }

        public DateTime? NotificationReceived { get; set; }

        public DateTime? PaymentReceived { get; set; }

        public DateTime? AssessmentStarted { get; set; }

        public DateTime? ApplicationCompleted { get; set; }

        public DateTime? Acknowledged { get; set; }

        public DateTime? DecisionDate { get; set; }

        public DateTime? DecisionRequiredDate { get; set; }

        public int? AssessmentStartedElapsedWorkingDays { get; set; }

        public int? ReceivedToAcknowledgedElapsedWorkingDays { get; set; }

        public int? CompleteToAcknowledgedElapsedWorkingDays { get; set; }

        public int? ReceivedToConsentElapsedWorkingDays { get; set; }

        public int? DecisionToConsentElapsedWorkingDays { get; set; }

        public string Officer { get; set; }
    }
}