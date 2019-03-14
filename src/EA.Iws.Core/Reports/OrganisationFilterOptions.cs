using System.ComponentModel.DataAnnotations;

namespace EA.Iws.Core.Reports
{
    public enum OrganisationFilterOptions
    {
        [Display(Name = "Notifier name")]
        Notifier,

        [Display(Name = "Consignee name")]
        Consignee
    }
}
