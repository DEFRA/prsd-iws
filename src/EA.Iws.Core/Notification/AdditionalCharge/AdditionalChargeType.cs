namespace EA.Iws.Core.Notification.AdditionalCharge
{
    using System.ComponentModel.DataAnnotations;

    public enum AdditionalChargeType
    {
        [Display(Name = "Edit export details", Description = "Edit export details")]
        EditExportDetails = 1,

        [Display(Name = "Edit producer details", Description = "Edit producer details")]
        EditProducerDetails = 2,

        [Display(Name = "Add producer details", Description = "Add producer details")]
        AddProducer = 3,

        [Display(Name = "Edit importer details", Description = "Edit importer details")]
        EditImporterDetails = 4,

        [Display(Name = "Edit consignee details", Description = "Edit consignee details")]
        EditConsigneeDetails = 5,

        [Display(Name = "Add carrier details", Description = "Add carrier details")]
        AddCarrier = 6,

        [Display(Name = "Update entry point details", Description = "Update entry point details")]
        UpdateEntryPoint = 7,

        [Display(Name = "Update exit point details", Description = "Update exit point details")]
        UpdateExitPoint = 8,

        [Display(Name = "Update number of shipment", Description = "Update number of shipment")]
        UpdateNumberOfShipment = 9,

        [Display(Name = "Accept notification", Description = "Accept notification")]
        AcceptNotification = 10,

        [Display(Name = "Resubmit notification", Description = "Resubmit notification")]
        ResubmitNotification = 11,

        [Display(Name = "Financial guarantee assessment changes", Description = "Financial guarantee assessment changes")]
        FinancialGuaranteeAssessment = 12,
    }
}
