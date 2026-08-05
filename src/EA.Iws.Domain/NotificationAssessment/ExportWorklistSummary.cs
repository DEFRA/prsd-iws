namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using Core.FinancialGuarantee;
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
        public DateTime? DecisionRequiredDate { get; private set; }
        public NotificationStatus Status { get; private set; }
        public DateTimeOffset? LastActionDate { get; private set; }
        public string LastActionType { get; private set; }
        public DateTimeOffset? LastCommentDate { get; private set; }
        public string FinancialGuaranteeStatus { get; private set; }
        public string LastComment { get; private set; }

        public static ExportWorklistSummary Load(
            Guid notificationId,
            string notificationNumber,
            string notifier,
            string officer,
            DateTime? datePickedUpByOfficer,
            DateTime? transmittedDate,
            DateTime? acknowledgedDate,
            DateTime? consentedDate,
            DateTime? decisionRequiredDate,
            NotificationStatus status,
            DateTimeOffset? lastActionDate,
            string lastActionType,
            DateTimeOffset? lastCommentDate,
            string financialGuaranteeStatus,
            string lastComment)
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
                DecisionRequiredDate = decisionRequiredDate,
                Status = status,
                LastActionDate = lastActionDate,
                LastActionType = lastActionType,
                LastCommentDate = lastCommentDate,
                FinancialGuaranteeStatus = financialGuaranteeStatus,
                LastComment = lastComment
            };
        }
    }
}