namespace EA.Iws.Core.NotificationAssessment
{
    using EA.Iws.Core.FinancialGuarantee;
    using System;

    public class ExportWorklistTableData
    {
        public Guid NotificationId { get; set; }

        public string NotificationNumber { get; set; }

        public string Notifier { get; set; }

        public string Officer { get; set; }

        public DateTime? DatePickedUpByOfficer { get; set; }

        public int? WorkingDaysInAssessment { get; set; }

        public DateTime? TransmittedDate { get; set; }

        public DateTime? AcknowledgedDate { get; set; }

        public DateTime? DecisionRequiredDate { get; set; }

        public string DaysRemaining { get; set; }

        public DateTime? ConsentedDate { get; set; }

        public DateTimeOffset? LastActionDate { get; set; }

        public string LastActionType { get; set; }

        public DateTimeOffset? LastCommentDate { get; set; }

        public NotificationStatus Status { get; set; }

        public string FinancialGuaranteeStatus { get; set; }

        public string LastComment { get; set; }
        public string LastCommentUser { get; set; }
    }
}