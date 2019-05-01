namespace EA.Iws.Core.Reports.Compliance
{
    using System.ComponentModel.DataAnnotations;

    public enum ComplianceReportDates
    {
        [Display(Name = "Consent valid from date")]
        ConsentFrom,

        [Display(Name = "Consent valid to date")]
        ConsentTo,

        [Display(Name = "Notification received date")]
        NotificationReceivedDate
    }
}
