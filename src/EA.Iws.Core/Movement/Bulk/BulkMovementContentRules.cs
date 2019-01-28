namespace EA.Iws.Core.Movement.Bulk
{
    using System.ComponentModel.DataAnnotations;

    public enum BulkMovementContentRules
    {
        [Display(Name = "Shipment number/s {0}: there is missing data")]
        MissingData,
        [Display(Name = "You can't create {0} shipments as there are only {1} active loads remaining.")]
        ExcessiveShipments
    }
}
