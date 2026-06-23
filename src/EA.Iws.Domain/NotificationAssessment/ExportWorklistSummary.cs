namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using Core.NotificationAssessment;

    public class ExportWorklistSummary
    {
        public Guid NotificationId { get; private set; }

        public string NotificationNumber { get; private set; }

        public string Notifier { get; private set; }

        public string Officer { get; private set; }

        public DateTime? DatePickedUpByOfficer { get; private set; }

        public DateTime? TransmittedDate { get; private set; }

        public DateTime? AcknowledgedDate { get; private set; }

        public DateTime? ConsentedDate { get; private set; }

        public DateTime? DecisionRequiredByDateOverride { get; private set; }

        public NotificationStatus Status { get; private set; }

        public DateTimeOffset? LastAuditDate { get; private set; }

        public string LastAuditType { get; private set; }

        public DateTimeOffset? LastCommentDate { get; private set; }

        public static ExportWorklistSummary Load(
            Guid notificationId,
            string notificationNumber,
            string notifier,
            string officer,
            DateTime? datePickedUpByOfficer,
            DateTime? transmittedDate,
            DateTime? acknowledgedDate,
            DateTime? consentedDate,
            DateTime? decisionRequiredByDateOverride,
            NotificationStatus status,
            DateTimeOffset? lastAuditDate,
            string lastAuditType,
            DateTimeOffset? lastCommentDate)
        {
            return new ExportWorklistSummary
            {
                NotificationId = notificationId,
                NotificationNumber = notificationNumber,
                Notifier = notifier,
                Officer = officer,
                DatePickedUpByOfficer = datePickedUpByOfficer,
                TransmittedDate = transmittedDate,
                AcknowledgedDate = acknowledgedDate,
                ConsentedDate = consentedDate,
                DecisionRequiredByDateOverride = decisionRequiredByDateOverride,
                Status = status,
                LastAuditDate = lastAuditDate,
                LastAuditType = lastAuditType,
                LastCommentDate = lastCommentDate
            };
        }
    }
}