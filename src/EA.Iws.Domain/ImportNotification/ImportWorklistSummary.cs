namespace EA.Iws.Domain.ImportNotificationAssessment
{
    using System;
    using Core.ImportNotificationAssessment;

    public class ImportWorklistSummary
    {
        public Guid NotificationId { get; private set; }
        public string NotificationNumber { get; private set; }
        public string Exporter { get; private set; }
        public string Officer { get; private set; }
        public DateTime? DatePickedUpByOfficer { get; private set; }
        public DateTime? NotificationReceivedDate { get; private set; }
        public DateTime? AcknowledgedDate { get; private set; }
        public DateTime? ConsentedDate { get; private set; }
        public DateTime? DecisionRequiredDate { get; private set; }
        public ImportNotificationStatus Status { get; private set; }
        public DateTimeOffset? LastCommentDate { get; private set; }

        public static ImportWorklistSummary Load(
            Guid notificationId,
            string notificationNumber,
            string exporter,
            string officer,
            DateTime? datePickedUpByOfficer,
            DateTime? notificationReceivedDate,
            DateTime? acknowledgedDate,
            DateTime? consentedDate,
            DateTime? decisionRequiredDate,
            ImportNotificationStatus status,
            DateTimeOffset? lastCommentDate)
        {
            return new ImportWorklistSummary
            {
                NotificationId = notificationId,
                NotificationNumber = notificationNumber,
                Exporter = exporter,
                Officer = officer,
                DatePickedUpByOfficer = datePickedUpByOfficer,
                NotificationReceivedDate = notificationReceivedDate,
                AcknowledgedDate = acknowledgedDate,
                ConsentedDate = consentedDate,
                DecisionRequiredDate = decisionRequiredDate,
                Status = status,
                LastCommentDate = lastCommentDate
            };
        }
    }
}