namespace EA.Iws.Core.Movement.Bulk
{
    using System.ComponentModel.DataAnnotations;

    public enum BulkMovementContentRules
    {
        [Display(Name = "Shipment number {0}: the actual date of shipment can't be outside of the consent validity period.")]
        ConsentValidity,
        [Display(Name = "Shipment number {0}: is duplicated within the data file.")]
        DuplicateShipmentNumber,
        [Display(Name = "You can't create {0} shipments as there are only {1} active loads remaining.")]
        ExcessiveShipments,
        [Display(Name = "Shipment number {0}: the date of shipment must not be historic.")]
        HistoricDate,
        [Display(Name = "Shipment number {0}: the date of shipment format must be in dd/mm/yyyy format.")]
        InvalidDateFormat,
        [Display(Name = "Shipment number {0}: there is missing data.")]
        MissingData,
        [Display(Name = "You can't create {0} shipment(s) as they are missing a value for shipment number.")]
        MissingShipmentNumbers,
        [Display(Name = "Shipment number {0}: data must only be for notification number {1}")]
        WrongNotificationNumber,
        [Display(Name = "Shipment number {0}: this shipment number already exists.")]
        OnlyNewShipments,
        [Display(Name = "Shipment number {0}: the shipment number is invalid - you've reached your shipment limit.")]
        InvalidShipmentNumber,
        [Display(Name = "Shipment {0}: the quantity of waste will exceed your permitted allowance and can't be prenotified.")]
        QuantityExceeded,
        [Display(Name = "Shipment {0}: the actual date of shipment is beyond your permitted Consent Window and therefore you are not allowed to prenotify this shipment.")]
        BeyondConsentWindow,
        [Display(Name = "Shipment number {0}: the packaging type is not permitted on this notification.")]
        InvalidPackagingType,
        [Display(Name = "Shipment number {0}: the waste quantity should have no more than 4 decimal places.")]
        QuantityPrecision,
        [Display(Name = "Shipment number {0}: the quantity unit of measurement is not permitted on this notification.")]
        QuantityUnit,
        [Display(Name = "Shipment number {0}: the date of shipment is less than three working days from the notification expiry date. Remove the shipment data from the data file and use the existing ‘generate a prenotification’ journey to inform us of this shipment.")]
        ThreeWorkingDaysToConsentDate,
        [Display(Name = "Shipment {0}: the actual shipment date is less than 3 working days.")]
        ThreeWorkingDaysToShipment,
        [Display(Name = "We think the first row was header data and have removed it. If the data was not header data, please check the first row and try to upload again")]
        HeaderDataRemoved,
    }
}
