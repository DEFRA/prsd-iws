namespace EA.Iws.Core.Shared
{
    using System.ComponentModel.DataAnnotations;

    public enum BusinessType
    {
        [Display(Name = "Limited company")]
        LimitedCompany = 1,

        [Display(Name = "Sole trader")]
        SoleTrader = 2,

        [Display(Name = "Partnership")]
        Partnership = 3,

        [Display(Name = "Other")]
        Other = 4
    }
}