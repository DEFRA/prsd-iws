namespace EA.Iws.RequestHandlers.Notification
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Notification;
    using DataAccess;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Shipment;
    using Domain.NotificationApplication.WasteRecovery;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using Requests.Notification.Overview;

    internal class GetNotificationOverviewHandler : IRequestHandler<GetNotificationOverview, NotificationOverview>
    {
        private readonly IwsContext db;
        private readonly NotificationChargeCalculator notificationChargeCalculator;
        private readonly IShipmentInfoRepository shipmentInfoRepository;
        private readonly IMap<NotificationApplication, ShipmentOverview> shipmentMap;
        private readonly IMap<NotificationApplication, WasteClassificationOverview> wasteClassificationMap;
        private readonly IMap<WasteRecovery, WasteRecoveryOverview> wasteRecoveryMap;
        private readonly IMap<NotificationApplication, OrganisationsInvolved> organisationsInvolvedInfoMap;
        private readonly IMap<NotificationApplication, RecoveryOperation> recoveryOperationInfoMap;
        private readonly IMap<NotificationApplication, Transportation> transportationInfoMap;
        private readonly IMap<NotificationApplication, Journey> journeyInfoMap;
        private readonly IMap<NotificationApplication, SubmitSummaryData> submitSummaryDataMap;
        private readonly IMap<NotificationApplication, WasteCodesOverviewInfo> wasteCodesOverviewMap;
        private readonly INotificationProgressService progressService;
        private readonly IMap<WasteDisposal, WasteDisposalOverview> wasteDisposalMap;

        public GetNotificationOverviewHandler(IwsContext db,
            NotificationChargeCalculator notificationChargeCalculator,
            INotificationProgressService progressService,
            IShipmentInfoRepository shipmentInfoRepository,
            IMap<NotificationApplication, ShipmentOverview> shipmentMap,
            IMap<NotificationApplication, WasteClassificationOverview> wasteClassificationMap,
            IMap<NotificationApplication, OrganisationsInvolved> organisationsInvolvedInfoMap,
            IMap<NotificationApplication, RecoveryOperation> recoveryOperationInfoMap,
            IMap<NotificationApplication, Transportation> transportationInfoMap,
            IMap<NotificationApplication, Journey> journeyInfoMap,
            IMap<NotificationApplication, SubmitSummaryData> submitSummaryDataMap,
            IMap<NotificationApplication, WasteCodesOverviewInfo> wasteCodesOverviewMap,
            IMap<WasteRecovery, WasteRecoveryOverview> wasteRecoveryMap,
            IMap<WasteDisposal, WasteDisposalOverview> wasteDisposalMap)
        {
            this.wasteDisposalMap = wasteDisposalMap;
            this.db = db;
            this.notificationChargeCalculator = notificationChargeCalculator;
            this.progressService = progressService;
            this.shipmentInfoRepository = shipmentInfoRepository;
            this.shipmentMap = shipmentMap;
            this.wasteClassificationMap = wasteClassificationMap;
            this.organisationsInvolvedInfoMap = organisationsInvolvedInfoMap;
            this.recoveryOperationInfoMap = recoveryOperationInfoMap;
            this.transportationInfoMap = transportationInfoMap;
            this.journeyInfoMap = journeyInfoMap;
            this.submitSummaryDataMap = submitSummaryDataMap;
            this.wasteCodesOverviewMap = wasteCodesOverviewMap;
            this.wasteRecoveryMap = wasteRecoveryMap;
        }

        public async Task<NotificationOverview> HandleAsync(GetNotificationOverview message)
        {
            var pricingStructures = await db.PricingStructures.ToArrayAsync();
            var shipmentInfo = await shipmentInfoRepository.GetByNotificationId(message.NotificationId);

            var overviewDataQuery = 
                from notification in db.NotificationApplications
                where notification.Id == message.NotificationId
                from wasteRecovery
                //left join waste recovery, if it exists
                in db.WasteRecoveries
                    .Where(wr => wr.NotificationId == notification.Id)
                    .DefaultIfEmpty()
                from assessment
                //left join assessment, if it exists
                in db.NotificationAssessments
                    .Where(na => na.NotificationApplicationId == notification.Id)
                    .DefaultIfEmpty()
                from wasteDiposal
                in db.WasteDisposals
                    .Where(wd => wd.NotificationId == notification.Id)
                    .DefaultIfEmpty()
                select new
                {
                    Notification = notification,
                    WasteRecovery = wasteRecovery,
                    WasteDisposal = wasteDiposal,
                    NotificationAssessment = assessment
                };

            var overviewData = await overviewDataQuery.SingleOrDefaultAsync();
            
            //TODO: as each domain entity is removed from the notification, each map below can be changed so that it doesn't
            // map the whole notification to the sub-overview object. The above query will need way more joins...

            return new NotificationOverview
            {
                NotificationType = (Core.Shared.NotificationType)overviewData.Notification.NotificationType.Value,
                Progress = progressService.GetNotificationProgressInfo(message.NotificationId),
                ShipmentOverview = shipmentMap.Map(overviewData.Notification),
                WasteClassificationOverview = wasteClassificationMap.Map(overviewData.Notification),
                CompetentAuthority = (CompetentAuthority)overviewData.Notification.CompetentAuthority.Value,
                WasteRecovery = wasteRecoveryMap.Map(overviewData.WasteRecovery),
                WasteDisposal = wasteDisposalMap.Map(overviewData.WasteDisposal),
                NotificationNumber = overviewData.Notification.NotificationNumber,
                NotificationId = overviewData.Notification.Id,
                Journey = journeyInfoMap.Map(overviewData.Notification),
                RecoveryOperation = recoveryOperationInfoMap.Map(overviewData.Notification),
                OrganisationsInvolved = organisationsInvolvedInfoMap.Map(overviewData.Notification),
                Transportation = transportationInfoMap.Map(overviewData.Notification),
                SubmitSummaryData = submitSummaryDataMap.Map(overviewData.Notification),
                WasteCodesOverview = wasteCodesOverviewMap.Map(overviewData.Notification),
                CanEditNotification = overviewData.NotificationAssessment.CanEditNotification,
                NotificationCharge = decimal.ToInt32(
                    notificationChargeCalculator.GetValue(pricingStructures, overviewData.Notification, shipmentInfo))
            };
        }
    }
}