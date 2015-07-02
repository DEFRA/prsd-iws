namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.CustomsOffice;
    using Core.MeansOfTransport;
    using Core.Notification;
    using Core.RecoveryInfo;
    using Core.Shared;
    using Core.Shipment;
    using Core.StateOfExport;
    using Core.StateOfImport;
    using Core.TransitState;
    using Core.WasteCodes;
    using Core.WasteType;
    using Requests.Notification;

    public class NotificationOverviewViewModel
    {
        public string NotificationNumber { get; set; }

        public Guid NotificationId { get; set; }

        public NotificationType NotificationType { get; set; }

        public NotificationApplicationCompletionProgress Progress { get; set; }

        public string ExporterCompanyName { get; set; }

        public List<string> ProducersCompanyNames { get; set; }

        public string ImporterCompanyName { get; set; }

        public List<string> FacilitiesCompanyNames { get; set; }

        public string PreconstedAnswer { get; set; }

        public List<string> OperationCodes { get; set; }

        public string TechnologyEmployed { get; set; }

        public string ReasonForExport { get; set; }

        public List<string> CariersCompanyNames { get; set; }

        public List<MeansOfTransport> MeanOfTransport { get; set; }

        public List<string> PackagingData { get; set; }

        public string SpecialHandlingDetails { get; set; }

        public StateOfExportData StateOfExportData { get; set; }

        public List<TransitStateData> TransitStates { get; set; }

        public StateOfImportData StateOfImportData { get; set; }

        public CustomsOfficeData EntryCustomsOffice { get; set; }

        public CustomsOfficeData ExitCustomsOffice { get; set; }

        public ShipmentData ShipmentData { get; set; }

        public WasteTypeData ChemicalComposition { get; set; }

        public string ProcessOfGeneration { get; set; }

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

        public NotificationOverviewViewModel()
        {
        }

        public NotificationOverviewViewModel(NotificationInfo notificationInfo)
        {
            NotificationId = notificationInfo.NotificationId;
            NotificationNumber = notificationInfo.NotificationNumber;
            NotificationType = notificationInfo.NotificationType;
            Progress = notificationInfo.Progress;
            ExporterCompanyName = notificationInfo.ExporterCompanyName;
            ProducersCompanyNames = notificationInfo.ProducersCompanyNames;
            ImporterCompanyName = notificationInfo.ImporterCompanyName;
            FacilitiesCompanyNames = notificationInfo.FacilitiesCompanyNames;
            PreconstedAnswer = notificationInfo.PreconstedAnswer;
            OperationCodes = notificationInfo.OperationCodes.OrderBy(c => c.Value).Select(c => c.Code).ToList();
            TechnologyEmployed = notificationInfo.TechnologyEmployed.AnnexProvided ? "The details will be provided in a separate document" : notificationInfo.TechnologyEmployed.Details;
            ReasonForExport = notificationInfo.ReasonForExport;
            CariersCompanyNames = notificationInfo.CariersCompanyNames;
            MeanOfTransport = notificationInfo.MeanOfTransport;
            PackagingData = notificationInfo.PackagingData;
            SpecialHandlingDetails = notificationInfo.SpecialHandlingDetails;
            StateOfExportData = notificationInfo.TransportRoute.StateOfExport;
            TransitStates = notificationInfo.TransportRoute.TransitStates.ToList();
            StateOfImportData = notificationInfo.TransportRoute.StateOfImport;
            EntryCustomsOffice = notificationInfo.EntryCustomsOffice.CustomsOfficeData ?? new CustomsOfficeData();
            ExitCustomsOffice = notificationInfo.ExitCustomsOffice.CustomsOfficeData ?? new CustomsOfficeData();
            ShipmentData = notificationInfo.ShipmentData ?? new ShipmentData();
            ChemicalComposition = notificationInfo.ChemicalComposition;
            ProcessOfGeneration = notificationInfo.ProcessOfGeneration.IsDocumentAttached ? "The details will be provided in a separate document" : notificationInfo.ProcessOfGeneration.Process;
            PhysicalCharacteristics = notificationInfo.PhysicalCharacteristics;
            BaselOecdCode = notificationInfo.BaselOecdCode;
            EwcCodes = notificationInfo.EwcCodes;
            NationExportCode = notificationInfo.NationExportCode;
            NationImportCode = notificationInfo.NationImportCode;
            OtherCodes = notificationInfo.OtherCodes;
            YCodes = notificationInfo.YCodes;
            HCodes = notificationInfo.HCodes;
            UnClass = notificationInfo.UnClass;
            UnNumber = notificationInfo.UnNumber;
            CustomCodes = notificationInfo.CustomCodes;
            RecoveryPercentageData = notificationInfo.RecoveryPercentageData;
            RecoveryInfoData = notificationInfo.RecoveryInfoData;
        }
    }
}