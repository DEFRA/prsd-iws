namespace EA.Iws.Core.Reports.Compliance
{
    using System.ComponentModel.DataAnnotations;
    public enum ComplianceTextFields
    {
        [Display(Name = "Notifier name")]
        Notifier,

        [Display(Name = "Consignee name")]
        Consignee,

        [Display(Name = "Waste type")]
        WasteType,

        [Display(Name = "EWC code")]
        EWCCode,

        [Display(Name = "Y code")]
        YCode,

        [Display(Name = "Point of exit")]
        PointOfExit,

        [Display(Name = "Point of entry")]
        PointOfEntry,

        [Display(Name = "Country of dispatch")]
        ExportCountryName,

        [Display(Name = "Country of destination")]
        ImportCountryName,

        [Display(Name = "Site of export name")]
        SiteOfExportName,

        [Display(Name = "Facility name")]
        FacilityName
    }
}
