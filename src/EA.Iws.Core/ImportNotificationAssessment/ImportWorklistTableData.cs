namespace EA.Iws.Core.ImportNotificationAssessment
{
    using System;

    public class ImportWorklistTableData
    {
        public Guid NotificationId { get; set; }
        public string NotificationNumber { get; set; }
        public string Exporter { get; set; }
        public string Officer { get; set; }
        public DateTime? DatePickedUpByOfficer { get; set; }
        public DateTime? NotificationReceivedDate { get; set; }
        public DateTime? AcknowledgedDate { get; set; }
        public DateTime? ConsentedDate { get; set; }
        public DateTime? DecisionRequiredDate { get; set; }
        public ImportNotificationStatus Status { get; set; }
        public int? WorkingDaysInAssessment { get; set; }
        public string DaysRemaining { get; set; }
        public DateTimeOffset? LastCommentDate { get; set; }
        public int? FinancialGuaranteeStatus { get; set; }
        public string FinancialGuaranteeStatusDescription { get; set; }
        public string LastAction { get; set; }
        public string LastCommentUser { get; set; }
    }
}