namespace EA.Iws.Core.Movement.BulkReceiptRecovery
{
    using System.ComponentModel.DataAnnotations;

    public enum ReceiptRecoveryContentRules
    {
        [Display(Name = "The maximum number of shipments that can be uploaded is {0}")]
        MaximumShipments,
        [Display(Name = "You can't create {0} shipment(s), there is missing notification and shipment number.")]
        MissingShipmentNumbers,
        [Display(Name = "Shipment number {0}: there is missing data.")]
        MissingData
    }
}
