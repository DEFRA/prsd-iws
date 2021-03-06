﻿namespace EA.Iws.Core.Admin.Reports
{
    using System;
    using System.ComponentModel;
    using NotificationAssessment;
    using Shared;

    public class DataExportNotificationData
    {
        public string NotificationNumber { get; set; }

        [DisplayName("Notification Type (R/D)")]
        public NotificationType NotificationType { get; set; }

        public NotificationStatus Status { get; set; }

        [DisplayName("Pre-consented")]
        public string Preconsented { get; set; }

        public DateTime? ConsentDate { get; set; }

        public DateTime? NotificationReceived { get; set; }

        public DateTime? PaymentReceived { get; set; }

        public DateTime? AssessmentStarted { get; set; }

        public DateTime? ApplicationCompleted { get; set; }

        public DateTime? Transmitted { get; set; }

        public DateTime? Acknowledged { get; set; }

        public DateTime? DecisionDate { get; set; }

        public DateTime? DecisionRequiredDate { get; set; }

        [DisplayName("Days to Assessment Started")]
        public int? AssessmentStartedElapsedWorkingDays { get; set; }

        [DisplayName("Days to Transmission")]
        public int? TransmittedElapsedWorkingDays { get; set; }

        public string Officer { get; set; }

        public string SubmittedBy { get; set; }

        [DisplayName("Consent valid To date")]
        public DateTime? ConsentTo { get; set; }
    }
}
