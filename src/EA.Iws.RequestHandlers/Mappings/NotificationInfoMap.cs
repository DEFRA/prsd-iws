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

        public NotificationInfoMap(
            IMap<NotificationApplication, NotificationApplicationCompletionProgress> completionProgressMap,
            IMap<NotificationApplication, OrganisationsInvolvedInfo> organisationsInvolvedInfoMap,
            IMap<NotificationApplication, RecoveryOperationInfo> recoveryOperationInfoMap,
            IMap<NotificationApplication, TransportationInfo> transportationInfoMap,
            IMap<NotificationApplication, JourneyInfo> journeyInfoMap,
            IMap<NotificationApplication, AmountsAndDatesInfo> amountsAndDatesInfoMap,
            IMap<NotificationApplication, ClassifyYourWasteInfo> classifyYourWasteInfoMap,
            IMap<NotificationApplication, WasteRecoveryInfo> wasteRecoveryInfoMap,
            IMap<NotificationApplication, SubmitSummaryData> submitSummaryDataMap)
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
        }

        public NotificationInfo Map(NotificationApplication notification)
        {
            var notificationCompletionProgress = completionProgressMap.Map(notification);
            var organisationsInvolvedInfo = organisationsInvolvedInfoMap.Map(notification);
            organisationsInvolvedInfo.IsExporterCompleted = notificationCompletionProgress.IsExporterCompleted;
            organisationsInvolvedInfo.IsProducerCompleted = notificationCompletionProgress.IsProducerCompleted;
            organisationsInvolvedInfo.IsImporterCompleted = notificationCompletionProgress.IsImporterCompleted;
            organisationsInvolvedInfo.IsFacilityCompleted = notificationCompletionProgress.IsFacilityCompleted;

            var recoveryOperationInfo = recoveryOperationInfoMap.Map(notification);
            recoveryOperationInfo.IsPreconsentStatusChosen = notificationCompletionProgress.IsPreconsentStatusChosen;
            recoveryOperationInfo.AreOperationCodesChosen = notificationCompletionProgress.AreOperationCodesChosen;
            recoveryOperationInfo.IsTechnologyEmployedCompleted = notificationCompletionProgress.IsTechnologyEmployedCompleted;
            recoveryOperationInfo.IsReasonForExportCompleted = notificationCompletionProgress.IsReasonForExportCompleted;

            var transportationInfo = transportationInfoMap.Map(notification);
            transportationInfo.IsCarrierCompleted = notificationCompletionProgress.IsCarrierCompleted;
            transportationInfo.IsMeansOfTransportCompleted = notificationCompletionProgress.IsMeansOfTransportCompleted;
            transportationInfo.IsPackagingTypesCompleted = notificationCompletionProgress.IsPackagingTypesCompleted;
            transportationInfo.IsSpecialHandlingCompleted = notificationCompletionProgress.IsSpecialHandlingCompleted;

            var journeyInfo = journeyInfoMap.Map(notification);
            journeyInfo.IsStateOfExportCompleted = notificationCompletionProgress.IsStateOfExportCompleted;
            journeyInfo.IsStateOfImportCompleted = notificationCompletionProgress.IsStateOfImportCompleted;
            journeyInfo.AreTransitStatesCompleted = notificationCompletionProgress.AreTransitStatesCompleted;
            journeyInfo.IsCustomsOfficeCompleted = notificationCompletionProgress.IsCustomsOfficeCompleted;

            var amountsAndDatesInfo = amountsAndDatesInfoMap.Map(notification);
            amountsAndDatesInfo.IsIntendedShipmentsCompleted = notificationCompletionProgress.IsIntendedShipmentsCompleted;

            var classifyYourWasteInfo = classifyYourWasteInfoMap.Map(notification);
            classifyYourWasteInfo.IsChemicalCompositionCompleted = notificationCompletionProgress.IsChemicalCompositionCompleted;
            classifyYourWasteInfo.IsProcessOfGenerationCompleted = notificationCompletionProgress.IsProcessOfGenerationCompleted;
            classifyYourWasteInfo.ArePhysicalCharacteristicsCompleted = notificationCompletionProgress.ArePhysicalCharacteristicsCompleted;
            classifyYourWasteInfo.AreWasteCodesCompleted = notificationCompletionProgress.AreWasteCodesCompleted;

            var wasteRecoveryInfo = wasteRecoveryInfoMap.Map(notification);
            wasteRecoveryInfo.IsWasteRecoveryInformationCompleted = notificationCompletionProgress.IsWasteRecoveryInformationCompleted;

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
                SubmitSummaryData = submitSummaryData
            };
        }
    }
}
