namespace EA.Iws.Core.Reports.FOI
{
    using System.ComponentModel.DataAnnotations;
    public enum FOIReportTextFields
    {
        [Display(Name = "Notifier name")]
        NotifierName,

        [Display(Name = "Producer name")]
        ProducerName,

        [Display(Name = "Consignee name")]
        ImporterName,

        [Display(Name = "Facility name")]
        FacilityName,

        [Display(Name = "Site of export name")]
        SiteOfExportName,

        [Display(Name = "Point of exit")]
        PointOfExport,

        [Display(Name = "Point of entry")]
        PointOfEntry,

        [Display(Name = "Country of dispatch")]
        ExportCountryName,

        [Display(Name = "Country of destination")]
        ImportCountryName,

        [Display(Name = "Waste type")]
        NameOfWaste,

        [Display(Name = "EWC code")]
        Ewc,

        [Display(Name = "Y code")]
        YCode     
    }
}