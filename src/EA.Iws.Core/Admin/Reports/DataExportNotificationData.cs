namespace EA.Iws.Core.Admin.Reports
{
    using System;
    using NotificationAssessment;
    using Shared;

    public class DataExportNotificationData
    {
        public string NotificationNumber { get; set; }

        public NotificationType NotificationType { get; set; }

        public NotificationStatus Status { get; set; }

        public DateTime? ConsentDate { get; set; }

        public DateTime? NotificationReceived { get; set; }

        public DateTime? PaymentReceived { get; set; }

        public DateTime? AssessmentStarted { get; set; }

        public DateTime? ApplicationCompleted { get; set; }

        public DateTime? Transmitted { get; set; }

        public DateTime? Acknowledged { get; set; }

        public DateTime? DecisionDate { get; set; }

        public DateTime? DecisionRequiredDate { get; set; }

        public int? AssessmentStartedElapsedWorkingDays { get; set; }

        public int? TransmittedElapsedWorkingDays { get; set; }

        public string Officer { get; set; }
    }
}
