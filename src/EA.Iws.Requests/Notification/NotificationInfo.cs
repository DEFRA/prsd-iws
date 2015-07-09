namespace EA.Iws.Requests.Notification
{
    using System;
    using System.Collections.Generic;
    using Core.MeansOfTransport;
    using Core.Notification;
    using Core.OperationCodes;
    using Core.RecoveryInfo;
    using Core.Shared;
    using Core.Shipment;
    using Core.TechnologyEmployed;
    using Core.WasteCodes;
    using Core.WasteType;
    using CustomsOffice;
    using StateOfExport;

    public class NotificationInfo
    {
        public Guid NotificationId { get; set; }

        public CompetentAuthority CompetentAuthority { get; set; }

        public string NotificationNumber { get; set; }

        public string CompetentAuthorityName { get; set; }

        public NotificationType NotificationType { get; set; }

        public NotificationApplicationCompletionProgress Progress { get; set; }

        public string ExporterCompanyName { get; set; }

        public List<string> ProducersCompanyNames { get; set; }

        public string ImporterCompanyName { get; set; }

        public List<string> FacilitiesCompanyNames { get; set; }

        public string PreconstedAnswer { get; set; }

        public List<OperationCodeData> OperationCodes { get; set; }

        public TechnologyEmployedData TechnologyEmployed { get; set; }

        public string ReasonForExport { get; set; }

        public List<string> CariersCompanyNames { get; set; }

        public List<MeansOfTransport> MeanOfTransport { get; set; }

        public List<string> PackagingData { get; set; }

        public string SpecialHandlingDetails { get; set; }

        public StateOfExportWithTransportRouteData TransportRoute { get; set; }

        public EntryCustomsOfficeAddData EntryCustomsOffice { get; set; }

        public ExitCustomsOfficeAddData ExitCustomsOffice { get; set; }

        public ShipmentData ShipmentData { get; set; }

        public WasteGenerationProcessData ProcessOfGeneration { get; set; }

        public List<string> PhysicalCharacteristics { get; set; }

        public WasteCodeData[] BaselOecdCode { get; set; }

        public WasteCodeData[] EwcCodes { get; set; }

        public WasteCodeData[] NationExportCode { get; set; }

        public WasteCodeData[] NationImportCode { get; set; }

        public WasteCodeData[] OtherCodes { get; set; }

        public WasteCodeData[] YCodes { get; set; }

        public WasteCodeData[] HCodes { get; set; }

        public WasteCodeData[] UnClass { get; set; }

        public WasteCodeData[] UnNumber { get; set; }

        public WasteCodeData[] CustomCodes { get; set; }

        public RecoveryPercentageData RecoveryPercentageData { get; set; }

        public RecoveryInfoData RecoveryInfoData { get; set; }

        public WasteTypeData ChemicalComposition { get; set; }

        public int NotificationCharge { get; set; }
    }
}