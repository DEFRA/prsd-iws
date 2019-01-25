namespace EA.Iws.Core.Movement.Bulk
{
    using System.ComponentModel.DataAnnotations;

    public enum BulkMovementContentRules
    {
        [Display(Name = "Shipment number/s {0}: there is missing data")]
        MissingData,
        [Display(Name = "You can't create {0} shipment(s) as they are missing a value for shipment number ")]
        MissingShipmentNumbers
    }
}
