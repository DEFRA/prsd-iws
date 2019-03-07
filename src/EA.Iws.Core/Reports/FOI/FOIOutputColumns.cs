namespace EA.Iws.Core.Reports.FOI
{
    using System.ComponentModel.DataAnnotations;
    public enum FOIOutputColumns
    {
        [Display(Name = "Import / Export")]
        ImportOrExport = 2,

        [Display(Name = "Interim / non-interim")]
        Interim = 3,

        [Display(Name = "Basel/OECD Code and Description")]
        BaselOecdCode,

        [Display(Name = "Notifier")]
        NotifierName,

        [Display(Name = "Notifier Address")]
        NotifierAddress,

        [Display(Name = "Notifier Postal Code")]
        NotifierPostalCode,

        [Display(Name = "Notifier type")]
        NotifierType,

        [Display(Name = "Notifier contact name")]
        NotifierContactName,

        [Display(Name = "Notifier contact email address")]
        NotifierContactEmail,

        [Display(Name = "Producer")]
        ProducerName,

        [Display(Name = "Producer Address")]
        ProducerAddress,

        [Display(Name = "Producer Postal Code")]
        ProducerPostalCode,

        [Display(Name = "Producer type")]
        ProducerType,

        [Display(Name = "Producer contact email address")]
        ProducerContactEmail,

        [Display(Name = "Point of Exit")]
        PointOfExport,

        [Display(Name = "Point of Entry")]
        PointOfEntry,

        [Display(Name = "Country of Dispatch")]
        ExportCountryName,

        [Display(Name = "Country of Destination")]
        ImportCountryName,

        [Display(Name = "Countries of Transit")]
        TransitStates,

        [Display(Name = "Waste Type")]
        NameOfWaste,

        [Display(Name = "EWC Code")]
        Ewc,

        YCode,

        HCode,

        [Display(Name = "R/D Code(s)")]
        OperationCodes,

        [Display(Name = "Consignee")]
        ImporterName,

        [Display(Name = "Consignee Address")]
        ImporterAddress,

        [Display(Name = "Consignee Postal Code")]
        ImporterPostalCode,

        [Display(Name = "Consignee Type")]
        ImporterType,

        [Display(Name = "Consignee contact name")]
        ImporterContactName,

        [Display(Name = "Consignee contact email address")]
        ImporterContactEmail,

        [Display(Name = "Facility")]
        FacilityName,

        [Display(Name = "Facility Address")]
        FacilityAddress,

        [Display(Name = "Facility Postal Code")]
        FacilityPostalCode,

        [Display(Name = "Quantity Received")]
        QuantityReceived,

        [Display(Name = "Quantity Received Unit")]
        QuantityReceivedUnit,

        [Display(Name = "Intended Quantity")]
        IntendedQuantity,

        [Display(Name = "Intended Quantity Unit")]
        IntendedQuantityUnit,

        [Display(Name = "Consent Valid From")]
        ConsentFrom,

        [Display(Name = "Consent Valid To")]
        ConsentTo,

        [Display(Name = "Notification status")]
        NotificationStatus,

        [Display(Name = "Decision date")]
        DecisionRequiredByDate,

        [Display(Name = "Financial guarantee approved")]
        IsFinancialGuaranteeApproved,

        [Display(Name = "File closure date")]
        FileClosedDate,

        [Display(Name = "Area")]
        LocalArea,

        [Display(Name = "Technology employed")]
        TechnologyEmployed,
    }
}
