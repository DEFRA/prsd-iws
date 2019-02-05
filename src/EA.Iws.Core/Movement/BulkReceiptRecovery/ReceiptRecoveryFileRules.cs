namespace EA.Iws.Core.Movement.BulkReceiptRecovery
{
    using System.ComponentModel.DataAnnotations;

    public enum ReceiptRecoveryFileRules
    {
        [Display(Name = "Unable to read the file, format is invalid.")]
        FileParse,
        [Display(Name = "The file does not contain any data.")]
        EmptyData
    }
}
