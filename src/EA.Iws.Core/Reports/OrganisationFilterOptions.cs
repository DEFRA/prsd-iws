namespace EA.Iws.Core.Reports
{
    using System.ComponentModel.DataAnnotations;

    public enum OrganisationFilterOptions
    {
        [Display(Name = "Notifier name")]
        Exporter,

        [Display(Name = "Consignee name")]
        Importer
    }
}
