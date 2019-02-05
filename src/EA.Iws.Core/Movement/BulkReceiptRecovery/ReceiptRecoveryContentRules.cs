namespace EA.Iws.Core.Movement.BulkReceiptRecovery
{
    using System.ComponentModel.DataAnnotations;

    public enum ReceiptRecoveryContentRules
    {
        [Display(Name = "We think the first row was header data and have removed it. If the data was not header data, please check the first row and try to upload again. If the data was header data, and there are no errors, press continue.")]
        HeaderDataRemoved,
        [Display(Name = "Shipment number {0}: there is missing data.")]
        MissingData
    }
}
