namespace EA.Iws.RequestHandlers.Mappings
{
    using System.Linq;
    using Core.Notification;
    using DataAccess;
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
        private readonly IwsContext context;

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
            IMap<NotificationApplication, WasteCodesOverviewInfo> wasteCodesOverviewMap,
            IwsContext context)
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
            this.context = context;
        }

        public NotificationInfo Map(NotificationApplication notification)
        {
            var assessment = context.NotificationAssessments.Single(na => na.NotificationApplicationId == notification.Id);
            var notificationCompletionProgress = completionProgressMap.Map(notification);

            var organisationsInvolvedInfo = GetOrganisationsInvolvedInfo(notification, notificationCompletionProgress);
            var recoveryOperationInfo = GetRecoveryOperationInfo(notification, notificationCompletionProgress);
            var transportationInfo = GetTransportationInfo(notification, notificationCompletionProgress);
            var journeyInfo = GetJourneyInfo(notification, notificationCompletionProgress);
            var amountsAndDatesInfo = GetAmountsAndDatesInfo(notification, notificationCompletionProgress);
            var classifyYourWasteInfo = GetClassifyYourWasteInfo(notification, notificationCompletionProgress);
            var wasteCodesOverviewInfo = GetWasteCodesOverviewInfo(notification, notificationCompletionProgress);
            var wasteRecoveryInfo = GetWasteRecoveryInfo(notification, notificationCompletionProgress);
            var submitSummaryData = GetSubmitSummaryData(notification, notificationCompletionProgress);

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
                SubmitSummaryData = submitSummaryData,
                CanEditNotification = assessment.CanEditNotification
            };
        }

        private SubmitSummaryData GetSubmitSummaryData(NotificationApplication notification,
            NotificationApplicationCompletionProgress notificationCompletionProgress)
        {
            var submitSummaryData = submitSummaryDataMap.Map(notification);
            submitSummaryData.IsNotificationComplete = notificationCompletionProgress.IsAllComplete;
            return submitSummaryData;
        }

        private WasteRecoveryInfo GetWasteRecoveryInfo(NotificationApplication notification,
            NotificationApplicationCompletionProgress notificationCompletionProgress)
        {
            var wasteRecoveryInfo = wasteRecoveryInfoMap.Map(notification);
            wasteRecoveryInfo.IsWasteRecoveryInformationCompleted = notificationCompletionProgress.HasRecoveryData;
            return wasteRecoveryInfo;
        }

        private WasteCodesOverviewInfo GetWasteCodesOverviewInfo(NotificationApplication notification,
            NotificationApplicationCompletionProgress notificationCompletionProgress)
        {
            var wasteCodesOverviewInfo = wasteCodesOverviewMap.Map(notification);
            wasteCodesOverviewInfo.IsBaselOecdCodeCompleted = notificationCompletionProgress.HasBaselOecdCode;
            wasteCodesOverviewInfo.AreEwcCodesCompleted = notificationCompletionProgress.HasEwcCodes;
            wasteCodesOverviewInfo.AreYCodesCompleted = notificationCompletionProgress.HasYCodes;
            wasteCodesOverviewInfo.AreHCodesCompleted = notificationCompletionProgress.HasHCodes;
            wasteCodesOverviewInfo.AreUnClassesCompleted = notificationCompletionProgress.HasUnClasses;
            wasteCodesOverviewInfo.AreUnNumbersCompleted = notificationCompletionProgress.HasUnNumbers;
            wasteCodesOverviewInfo.AreOtherCodesCompleted = notificationCompletionProgress.HasOtherCodes;
            return wasteCodesOverviewInfo;
        }

        private ClassifyYourWasteInfo GetClassifyYourWasteInfo(NotificationApplication notification,
            NotificationApplicationCompletionProgress notificationCompletionProgress)
        {
            var classifyYourWasteInfo = classifyYourWasteInfoMap.Map(notification);
            classifyYourWasteInfo.IsChemicalCompositionCompleted = notificationCompletionProgress.HasWasteType;
            classifyYourWasteInfo.IsProcessOfGenerationCompleted = notificationCompletionProgress.HasWasteGenerationProcess;
            classifyYourWasteInfo.ArePhysicalCharacteristicsCompleted =
                notificationCompletionProgress.HasPhysicalCharacteristics;
            return classifyYourWasteInfo;
        }

        private AmountsAndDatesInfo GetAmountsAndDatesInfo(NotificationApplication notification,
            NotificationApplicationCompletionProgress notificationCompletionProgress)
        {
            var amountsAndDatesInfo = amountsAndDatesInfoMap.Map(notification);
            amountsAndDatesInfo.IsIntendedShipmentsCompleted = notificationCompletionProgress.HasShipmentInfo;
            return amountsAndDatesInfo;
        }

        private JourneyInfo GetJourneyInfo(NotificationApplication notification,
            NotificationApplicationCompletionProgress notificationCompletionProgress)
        {
            var journeyInfo = journeyInfoMap.Map(notification);
            journeyInfo.IsStateOfExportCompleted = notificationCompletionProgress.HasStateOfExport;
            journeyInfo.IsStateOfImportCompleted = notificationCompletionProgress.HasStateOfImport;
            journeyInfo.AreTransitStatesCompleted = notificationCompletionProgress.HasTransitState;
            journeyInfo.IsCustomsOfficeCompleted = notificationCompletionProgress.HasCustomsOffice;
            return journeyInfo;
        }

        private TransportationInfo GetTransportationInfo(NotificationApplication notification,
            NotificationApplicationCompletionProgress notificationCompletionProgress)
        {
            var transportationInfo = transportationInfoMap.Map(notification);
            transportationInfo.IsCarrierCompleted = notificationCompletionProgress.HasCarrier;
            transportationInfo.IsMeansOfTransportCompleted = notificationCompletionProgress.HasMeansOfTransport;
            transportationInfo.IsPackagingTypesCompleted = notificationCompletionProgress.HasPackagingInfo;
            transportationInfo.IsSpecialHandlingCompleted = notificationCompletionProgress.HasSpecialHandlingRequirements;
            return transportationInfo;
        }

        private RecoveryOperationInfo GetRecoveryOperationInfo(NotificationApplication notification,
            NotificationApplicationCompletionProgress notificationCompletionProgress)
        {
            var recoveryOperationInfo = recoveryOperationInfoMap.Map(notification);
            recoveryOperationInfo.IsPreconsentStatusChosen = notificationCompletionProgress.HasPreconsentedInformation;
            recoveryOperationInfo.AreOperationCodesChosen = notificationCompletionProgress.HasOperationCodes;
            recoveryOperationInfo.IsTechnologyEmployedCompleted = notificationCompletionProgress.HasTechnologyEmployed;
            recoveryOperationInfo.IsReasonForExportCompleted = notificationCompletionProgress.HasReasonForExport;
            return recoveryOperationInfo;
        }

        private OrganisationsInvolvedInfo GetOrganisationsInvolvedInfo(NotificationApplication notification,
            NotificationApplicationCompletionProgress notificationCompletionProgress)
        {
            var organisationsInvolvedInfo = organisationsInvolvedInfoMap.Map(notification);
            organisationsInvolvedInfo.IsExporterCompleted = notificationCompletionProgress.HasExporter;
            organisationsInvolvedInfo.IsProducerCompleted = notificationCompletionProgress.HasProducer;
            organisationsInvolvedInfo.IsImporterCompleted = notificationCompletionProgress.HasImporter;
            organisationsInvolvedInfo.IsFacilityCompleted = notificationCompletionProgress.HasFacility;
            organisationsInvolvedInfo.HasSiteOfExport = notificationCompletionProgress.HasSiteOfExport;
            organisationsInvolvedInfo.HasActualSiteOfTreatment = notificationCompletionProgress.HasActualSiteOfTreatment;
            return organisationsInvolvedInfo;
        }
    }
}
