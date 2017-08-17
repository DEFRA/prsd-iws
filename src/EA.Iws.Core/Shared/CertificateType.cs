namespace EA.Iws.Core.Shared
{
    using System.ComponentModel.DataAnnotations;
    public enum CertificateType
    {
        [Display(Name = "Receipt")]
        Receipt = 1,
        [Display(Name = "Recovery")]
        Recovery = 2,
        [Display(Name = "ReceiptRecovery")]
        ReceiptRecovery = 3
    }
}
