namespace EA.Iws.Core.FinancialGuarantee
{
    using System.ComponentModel.DataAnnotations;

    public enum FinancialGuaranteeDecision
    {
        None = 1,

        [Display(ShortName = "Refuse")]
        Refused = 2,

        [Display(ShortName = "Approve")]
        Approved = 3,

        [Display(ShortName = "Release")]
        Released = 4
    }
}