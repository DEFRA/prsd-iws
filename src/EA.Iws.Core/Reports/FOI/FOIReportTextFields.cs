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
        ConsigneeName,

        [Display(Name = "Facility name")]
        FacilityName,

        [Display(Name = "Site of export name")]
        SiteOfExportName,

        [Display(Name = "Point of exit")]
        PointOfExit,

        [Display(Name = "Point of entry")]
        PointOfEntry,

        [Display(Name = "Country of despatch")]
        CountryOfDespatch,

        [Display(Name = "Country of destination")]
        CountryOfDestination,

        [Display(Name = "Waste type")]
        WasteType,

        [Display(Name = "EWC code")]
        EwcCode,

        [Display(Name = "Y code")]
        YCode     
    }
}