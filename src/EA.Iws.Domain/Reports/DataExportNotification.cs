namespace EA.Iws.Domain.Reports
{
    using System;
    using Core.NotificationAssessment;
    using Core.Shared;

    public class DataExportNotification
    {
        public string NotificationNumber { get; protected set; }

        public NotificationType NotificationType { get; protected set; }

        public NotificationStatus Status { get; protected set; }

        public DateTime? NotificationReceived { get; protected set; }

        public DateTime? PaymentReceived { get; protected set; }

        public DateTime? AssessmentStarted { get; protected set; }

        public DateTime? ApplicationCompleted { get; protected set; }

        public DateTime? Transmitted { get; protected set; }

        public DateTime? Acknowledged { get; protected set; }

        public DateTime? DecisionDate { get; protected set; }

        public bool Preconsented { get; protected set; }
    }
}
