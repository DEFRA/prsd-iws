namespace EA.Iws.Core.Movement.Bulk
{
    using System.ComponentModel.DataAnnotations;

    public enum BulkMovementContentRules
    {
        [Display(Name = "Shipment number/s {0}: there is missing data")]
        MissingData,
        [Display(Name = "You can't create {0} shipments as there are only {1} active loads remaining.")]
        ExcessiveShipments,
        [Display(Name = "Shipment number {0}: is duplicated within the data file.")]
        DuplicateShipmentNumber,
        [Display(Name = "Shipment number {0}: the date of shipment must not be historic.")]
        HistoricDate,
        [Display(Name = "Shipment number {0}: the date of shipment format must be in dd/mm/yyyy format.")]
        InvalidDateFormat,
        [Display(Name = "You can't create {0} shipment(s) as they are missing a value for shipment number ")]
        MissingShipmentNumbers,
        [Display(Name = "Shipment number/s {0}: data must only be for notification number {1}")]
        WrongNotificationNumber,
        [Display(Name = "Shipment number {0}: this shipment number already exists.")]
        OnlyNewShipments
    }
}
