namespace EA.Iws.Core.Reports
{
    using System.ComponentModel.DataAnnotations;

    public enum ProducerReportDates
    {
        [Display(Name = "Consent valid from date")]
        ConsentFrom,

        [Display(Name = "Consent valid to date")]
        ConsentTo,

        [Display(Name = "Notification received date")]
        NotificationReceivedDate,

        [Display(Name = "Shipment received date")]
        ReceivedDate,

        [Display(Name = "Shipment recovered/disposed of date")]
        CompletedDate
    }
}
