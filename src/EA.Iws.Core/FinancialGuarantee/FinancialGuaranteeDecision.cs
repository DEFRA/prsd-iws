namespace EA.Iws.Core.FinancialGuarantee
{
    using System.ComponentModel.DataAnnotations;

    public enum FinancialGuaranteeDecision
    {
        None = 1,

        [Display(ShortName = "Refuse", Name = "Refused")]
        Refused = 2,

        [Display(ShortName = "Approve", Name = "Approved")]
        Approved = 3,

        [Display(ShortName = "Release", Name = "Released")]
        Released = 4
    }
}