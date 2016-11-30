namespace EA.Iws.Core.FinancialGuarantee
{
    using System.ComponentModel.DataAnnotations;

    public enum FinancialGuaranteeDecision
    {
        [Display(ShortName = "Refuse")]
        Refused = 0,

        [Display(ShortName = "Approve")]
        Approved = 1,

        [Display(ShortName = "Release")]
        Released = 2
    }
}