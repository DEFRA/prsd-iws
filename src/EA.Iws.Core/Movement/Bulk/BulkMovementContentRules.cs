namespace EA.Iws.Core.Movement.Bulk
{
    using System.ComponentModel.DataAnnotations;

    public enum BulkMovementContentRules
    {
        [Display(Name = "Shipment number {0}: is duplicated within the data file.")]
        DuplicateShipmentNumber,
        [Display(Name = "Shipment number {0}: the date of shipment must not be historic.")]
        HistoricDate,
        [Display(Name = "Shipment number {0}: the date of shipment format must be in dd/mm/yyyy format.")]
        InvalidDateFormat,
        [Display(Name = "Shipment number {0}: there is missing data")]
        MissingData,
        [Display(Name = "You can't create {0} shipment(s) as they are missing a value for shipment number ")]
        MissingShipmentNumbers
    }
}
