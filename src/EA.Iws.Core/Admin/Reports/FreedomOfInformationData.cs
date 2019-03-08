namespace EA.Iws.Core.Admin.Reports
{
    using System;
    using System.ComponentModel;

    public class FreedomOfInformationData
    {
        public string NotificationNumber { get; set; }

        [DisplayName("Import / Export")]
        public string ImportOrExport { get; set; }

        [DisplayName("Interim / non-interim")]
        public string Interim { get; set; }

        [DisplayName("Basel/OECD Code and Description")]
        public string BaselOecdCode { get; set; }

        [DisplayName("Notifier")]
        public string NotifierName { get; set; }

        public string NotifierAddress { get; set; }

        public string NotifierPostalCode { get; set; }

        public string NotifierType { get; set; }

        public string NotifierContactName { get; set; }

        public string NotifierContactEmail { get; set; }

        [DisplayName("Producer")]
        public string ProducerName { get; set; }

        public string ProducerAddress { get; set; }

        public string ProducerPostalCode { get; set; }

        public string ProducerType { get; set; }

        public string ProducerContactEmail { get; set; }

        [DisplayName("Point of Exit")]
        public string PointOfExport { get; set; }

        [DisplayName("Point of Entry")]
        public string PointOfEntry { get; set; }

        [DisplayName("Country of Dispatch")]
        public string ExportCountryName { get; set; }

        [DisplayName("Country of Destination")]
        public string ImportCountryName { get; set; }

        [DisplayName("Countries of Transit")]
        public string TransitStates { get; set; }

        [DisplayName("Waste Type")]
        public string NameOfWaste { get; set; }

        [DisplayName("EWC Code")]
        public string Ewc { get; set; }
        
        public string YCode { get; set; }

        public string HCode { get; set; }

        [DisplayName("R/D Code(s)")]
        public string OperationCodes { get; set; }

        [DisplayName("Consignee")]
        public string ImporterName { get; set; }

        [DisplayName("Consignee Address")]
        public string ImporterAddress { get; set; }

        [DisplayName("Consignee Postal Code")]
        public string ImporterPostalCode { get; set; }

        [DisplayName("Consignee Type")]
        public string ImporterType { get; set; }

        [DisplayName("Consignee contact name")]
        public string ImporterContactName { get; set; }

        [DisplayName("Consignee contact email address")]
        public string ImporterContactEmail { get; set; }

        [DisplayName("Facility")]
        public string FacilityName { get; set; }

        public string FacilityAddress { get; set; }

        public string FacilityPostalCode { get; set; }

        public decimal QuantityReceived { get; set; }

        public string QuantityReceivedUnit { get; set; }

        public decimal IntendedQuantity { get; set; }

        public string IntendedQuantityUnit { get; set; }

        [DisplayName("Consent Valid From")]
        public DateTime? ConsentFrom { get; set; }

        [DisplayName("Consent Valid To")]
        public DateTime? ConsentTo { get; set; }

        public string NotificationStatus { get; set; }

        [DisplayName("Decision date")]
        public DateTime? DecisionRequiredByDate { get; set; }

        [DisplayName("Financial guarantee approved ")]
        public string IsFinancialGuaranteeApproved { get; set; }

        [DisplayName("File closure date")]
        public DateTime? FileClosedDate { get; set; }

        [DisplayName("Area")]
        public string LocalArea { get; set; }

        public string TechnologyEmployed { get; set; }
    }
}