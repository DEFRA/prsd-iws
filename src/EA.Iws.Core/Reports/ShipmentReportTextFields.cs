namespace EA.Iws.Core.Reports
{
    using System.ComponentModel.DataAnnotations;

    public enum ShipmentReportTextFields
    {
        [Display(Name = "Notifier name")]
        Exporter,

        [Display(Name = "Consignee name")]
        Importer,

        [Display(Name = "Site of export name")]
        SiteOfExportName,

        [Display(Name = "Facility name")]
        Facility,

        [Display(Name = "Point of exit")]
        ExitPort,

        [Display(Name = "Point of entry")]
        EntryPort,

        [Display(Name = "Country of despatch")]
        OriginatingCountry,

        [Display(Name = "Country of destination")]
        DestinationCountry,

        [Display(Name = "Waste type")]
        ChemicalComposition,

        [Display(Name = "EWC code")]
        EwcCodes,

        [Display(Name = "Y code")]
        YCode
    }
}
