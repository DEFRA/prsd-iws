namespace EA.Iws.Core.Reports
{
    using System.ComponentModel.DataAnnotations;

    public enum MissingShipmentsReportDates
    {
        [Display(Name = "Notification received date")]
        NotificationReceivedDate,

        [Display(Name = "Consent valid from date")]
        ConsentFrom,

        [Display(Name = "Shipment received date")]
        ReceivedDate,

        [Display(Name = "Shipment recovered/disposed of date")]
        CompletedDate
    }
}
