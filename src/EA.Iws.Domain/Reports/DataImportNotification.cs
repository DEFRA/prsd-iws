namespace EA.Iws.Domain.Reports
{
    using System;
    using Core.ImportNotificationAssessment;
    using Core.Shared;

    public class DataImportNotification
    {
        public string NotificationNumber { get; protected set; }

        public NotificationType NotificationType { get; protected set; }

        public ImportNotificationStatus Status { get; protected set; }

        public DateTime? NotificationReceived { get; protected set; }

        public DateTime? PaymentReceived { get; protected set; }

        public DateTime? AssessmentStarted { get; protected set; }

        public DateTime? ApplicationCompleted { get; protected set; }

        public DateTime? Acknowledged { get; protected set; }

        public DateTime? DecisionDate { get; protected set; }

        public DateTime? Consented { get; protected set; }

        public string Officer { get; set; }

        public bool? Preconsented { get; protected set; }

        public DateTime? ConsentTo { get; set; }
    }
}
