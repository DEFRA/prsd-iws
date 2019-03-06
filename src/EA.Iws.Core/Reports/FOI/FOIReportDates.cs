namespace EA.Iws.Core.Reports.FOI
{
    using System.ComponentModel.DataAnnotations;
    public enum FOIReportDates
    {
        [Display(Name = "Notification received date")]
        NotificationReceivedDate,

        [Display(Name = "Consent valid from date")]
        ConsentFrom,

        [Display(Name = "Consent valid to date")]
        ConsentTo,

        [Display(Name = "Shipment received date")]
        ReceivedDate,

        [Display(Name = "Shipment recovered/disposed of date")]
        CompletedDate,

        [Display(Name = "Actual date of shipment")]
        ActualDate,

        [Display(Name = "Decision date")]
        DecisionDate,

        [Display(Name = "Acknowledged date")]
        AcknowledgedDate,

        [Display(Name = "Objection date")]
        ObjectionDate,

        [Display(Name = "File closed date")]
        FileClosedDate,

        [Display(Name = "Withdrawn date")]
        WithdrawnDate,
    }
}