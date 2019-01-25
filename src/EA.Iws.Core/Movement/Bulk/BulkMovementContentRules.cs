namespace EA.Iws.Core.Movement.Bulk
{
    using System.ComponentModel.DataAnnotations;

    public enum BulkMovementContentRules
    {
        [Display(Name = "Shipment number/s {0}: the date of shipment must not be historic.")]
        HistoricDate,
        [Display(Name = "Shipment number/s {0}: the date of shipment format must be in dd/mm/yyyy format.")]
        InvalidDateFormat,
        [Display(Name = "Shipment number/s {0}: there is missing data")]
        MissingData
    }
}
