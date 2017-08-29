namespace EA.Iws.Core.Reports
{
    using System.ComponentModel.DataAnnotations;

    public enum ShipmentsReportDates
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
        ActualDateOfShipment
    }
}
