namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.Notification;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Requests.Notification;

    internal class NotificationInfoMap : IMap<NotificationApplication, NotificationInfo>
    {
        private readonly IMap<NotificationApplication, NotificationApplicationCompletionProgress> completionProgressMap;
        private readonly IMap<NotificationApplication, OrganisationsInvolvedInfo> organisationsInvolvedInfoMap;
        private readonly IMap<NotificationApplication, RecoveryOperationInfo> recoveryOperationInfoMap;
        private readonly IMap<NotificationApplication, TransportationInfo> transportationInfoMap;
        private readonly IMap<NotificationApplication, JourneyInfo> journeyInfoMap;
        private readonly IMap<NotificationApplication, AmountsAndDatesInfo> amountsAndDatesInfoMap;
        private readonly IMap<NotificationApplication, ClassifyYourWasteInfo> classifyYourWasteInfoMap;
        private readonly IMap<NotificationApplication, WasteRecoveryInfo> wasteRecoveryInfoMap;
        private readonly IMap<NotificationApplication, SubmitSummaryData> submitSummaryDataMap;
        private readonly IMap<NotificationApplication, WasteCodesOverviewInfo> wasteCodesOverviewMap;

        public NotificationInfoMap(
            IMap<NotificationApplication, NotificationApplicationCompletionProgress> completionProgressMap,
            IMap<NotificationApplication, OrganisationsInvolvedInfo> organisationsInvolvedInfoMap,
            IMap<NotificationApplication, RecoveryOperationInfo> recoveryOperationInfoMap,
            IMap<NotificationApplication, TransportationInfo> transportationInfoMap,
            IMap<NotificationApplication, JourneyInfo> journeyInfoMap,
            IMap<NotificationApplication, AmountsAndDatesInfo> amountsAndDatesInfoMap,
            IMap<NotificationApplication, ClassifyYourWasteInfo> classifyYourWasteInfoMap,
            IMap<NotificationApplication, WasteRecoveryInfo> wasteRecoveryInfoMap,
            IMap<NotificationApplication, SubmitSummaryData> submitSummaryDataMap,
            IMap<NotificationApplication, WasteCodesOverviewInfo> wasteCodesOverviewMap)
        {
            this.completionProgressMap = completionProgressMap;
            this.organisationsInvolvedInfoMap = organisationsInvolvedInfoMap;
            this.recoveryOperationInfoMap = recoveryOperationInfoMap;
            this.transportationInfoMap = transportationInfoMap;
            this.journeyInfoMap = journeyInfoMap;
            this.amountsAndDatesInfoMap = amountsAndDatesInfoMap;
            this.wasteRecoveryInfoMap = wasteRecoveryInfoMap;
            this.classifyYourWasteInfoMap = classifyYourWasteInfoMap;
            this.submitSummaryDataMap = submitSummaryDataMap;
            this.wasteCodesOverviewMap = wasteCodesOverviewMap;
        }

        public NotificationInfo Map(NotificationApplication notification)
        {
            var notificationCompletionProgress = completionProgressMap.Map(notification);
            var organisationsInvolvedInfo = organisationsInvolvedInfoMap.Map(notification);
            organisationsInvolvedInfo.IsExporterCompleted = notificationCompletionProgress.HasExporter;
            organisationsInvolvedInfo.IsProducerCompleted = notificationCompletionProgress.HasProducer;
            organisationsInvolvedInfo.IsImporterCompleted = notificationCompletionProgress.HasImporter;
            organisationsInvolvedInfo.IsFacilityCompleted = notificationCompletionProgress.HasFacility;
            organisationsInvolvedInfo.HasSiteOfExport = notificationCompletionProgress.HasSiteOfExport;
            organisationsInvolvedInfo.HasActualSiteOfTreatment = notificationCompletionProgress.HasActualSiteOfTreatment;

            var recoveryOperationInfo = recoveryOperationInfoMap.Map(notification);
            recoveryOperationInfo.IsPreconsentStatusChosen = notificationCompletionProgress.HasPreconsentedInformation;
            recoveryOperationInfo.AreOperationCodesChosen = notificationCompletionProgress.HasOperationCodes;
            recoveryOperationInfo.IsTechnologyEmployedCompleted = notificationCompletionProgress.HasTechnologyEmployed;
            recoveryOperationInfo.IsReasonForExportCompleted = notificationCompletionProgress.HasReasonForExport;

            var transportationInfo = transportationInfoMap.Map(notification);
            transportationInfo.IsCarrierCompleted = notificationCompletionProgress.HasCarrier;
            transportationInfo.IsMeansOfTransportCompleted = notificationCompletionProgress.HasMeansOfTransport;
            transportationInfo.IsPackagingTypesCompleted = notificationCompletionProgress.HasPackagingInfo;
            transportationInfo.IsSpecialHandlingCompleted = notificationCompletionProgress.HasSpecialHandlingRequirements;

            var journeyInfo = journeyInfoMap.Map(notification);
            journeyInfo.IsStateOfExportCompleted = notificationCompletionProgress.HasStateOfExport;
            journeyInfo.IsStateOfImportCompleted = notificationCompletionProgress.HasStateOfImport;
            journeyInfo.AreTransitStatesCompleted = notificationCompletionProgress.HasTransitState;
            journeyInfo.IsCustomsOfficeCompleted = notificationCompletionProgress.HasCustomsOffice;

            var amountsAndDatesInfo = amountsAndDatesInfoMap.Map(notification);
            amountsAndDatesInfo.IsIntendedShipmentsCompleted = notificationCompletionProgress.HasShipmentInfo;

            var classifyYourWasteInfo = classifyYourWasteInfoMap.Map(notification);
            classifyYourWasteInfo.IsChemicalCompositionCompleted = notificationCompletionProgress.HasWasteType;
            classifyYourWasteInfo.IsProcessOfGenerationCompleted = notificationCompletionProgress.HasWasteGenerationProcess;
            classifyYourWasteInfo.ArePhysicalCharacteristicsCompleted = notificationCompletionProgress.HasPhysicalCharacteristics;

            var wasteCodesOverviewInfo = wasteCodesOverviewMap.Map(notification);
            wasteCodesOverviewInfo.AreWasteCodesCompleted = notificationCompletionProgress.HasWasteCodes;

            var wasteRecoveryInfo = wasteRecoveryInfoMap.Map(notification);
            wasteRecoveryInfo.IsWasteRecoveryInformationCompleted = notificationCompletionProgress.HasRecoveryData;

            var submitSummaryData = submitSummaryDataMap.Map(notification);
            submitSummaryData.IsNotificationComplete = notificationCompletionProgress.IsAllComplete;

            return new NotificationInfo
            {
                NotificationId = notification.Id,
                CompetentAuthority = (CompetentAuthority)notification.CompetentAuthority.Value,
                NotificationNumber = notification.NotificationNumber,
                OrganisationsInvolvedInfo = organisationsInvolvedInfo,
                RecoveryOperationInfo = recoveryOperationInfo,
                TransportationInfo = transportationInfo,
                JourneyInfo = journeyInfo,
                AmountsAndDatesInfo = amountsAndDatesInfo,
                ClassifyYourWasteInfo = classifyYourWasteInfo,
                WasteRecoveryInfo = wasteRecoveryInfo,
                WasteCodesOverviewInfo = wasteCodesOverviewInfo,
                SubmitSummaryData = submitSummaryData
            };
        }
    }
}
