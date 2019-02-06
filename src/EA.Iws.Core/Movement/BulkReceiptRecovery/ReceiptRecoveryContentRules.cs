namespace EA.Iws.Core.Movement.BulkReceiptRecovery
{
    using System.ComponentModel.DataAnnotations;

    public enum ReceiptRecoveryContentRules
    {
        [Display(Name = "The maximum number of shipments that can be uploaded is {0}")]
        MaximumShipments,
        [Display(Name = "You can't create {0} shipment(s) as they have their shipment numbers missing.")]
        MissingShipmentNumbers,
        [Display(Name = "Shipment number {0}: the notification number is missing.")]
        MissingNotificationNumber,
        [Display(Name = "Shipment number {0}: all three parts of the receipt data need to be present.")]
        MissingReceiptData
    }
}
