namespace EA.Iws.Core.Reports
{
    using System.ComponentModel.DataAnnotations;

    public enum OrganisationFilterOptions
    {
        [Display(Name = "Notifier name")]
        Notifier,

        [Display(Name = "Consignee name")]
        Consignee
    }
}
