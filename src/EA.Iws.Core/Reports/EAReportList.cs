namespace EA.Iws.Core.Reports
{
    using System.ComponentModel.DataAnnotations;

    public enum EAReportList
    {
        [Display(Name = "ShipmentReport", Description = "Shipment report")]
        ShipmentReport = 1,
        [Display(Name = "FinanceReport", Description = "Finance report")]
        FinanceReport = 2,
        [Display(Name = "ProducerReport", Description = "Producer report")]
        ProducerReport = 3,
        [Display(Name = "FOIReport", Description = "FOI report")]
        FOIReport = 4,
    }
}
