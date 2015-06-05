namespace EA.Iws.Requests.Shared
{
    using System.ComponentModel.DataAnnotations;

    public enum BusinessType
    {
        [Display(Name = "Limited Company")]
        LimitedCompany = 1,

        [Display(Name = "Sole Trader")]
        SoleTrader = 2,

        [Display(Name = "Parthership")]
        Partnership = 3,

        [Display(Name = "Other")]
        Other = 4
    }
}