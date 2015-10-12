namespace EA.Iws.RequestHandlers.Notification
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Notification;
    using Core.Notification.Overview;
    using DataAccess;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Shipment;
    using Domain.NotificationApplication.WasteRecovery;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GetNotificationOverviewHandler : IRequestHandler<GetNotificationOverview, NotificationOverview>
    {
        private readonly IwsContext db;
        private readonly NotificationChargeCalculator notificationChargeCalculator;
        private readonly INotificationProgressService progressService;
        private readonly IShipmentInfoRepository shipmentInfoRepository;
        private readonly IMapper mapper;

        public GetNotificationOverviewHandler(IwsContext db,
            NotificationChargeCalculator notificationChargeCalculator,
            INotificationProgressService progressService,
            IShipmentInfoRepository shipmentInfoRepository,
            IMapper mapper)
        {
            this.db = db;
            this.notificationChargeCalculator = notificationChargeCalculator;
            this.progressService = progressService;
            this.shipmentInfoRepository = shipmentInfoRepository;
            this.mapper = mapper;
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

            var overviewData = await overviewDataQuery.SingleAsync();

            //TODO: as each domain entity is removed from the notification, each map below can be changed so that it doesn't
            // map the whole notification to the sub-overview object. The above query will need way more joins...

            return new NotificationOverview
            {
                NotificationType = (Core.Shared.NotificationType)overviewData.Notification.NotificationType.Value,
                Progress = progressService.GetNotificationProgressInfo(message.NotificationId),
                ShipmentOverview = mapper.Map<NotificationApplication, ShipmentOverview>(overviewData.Notification),
                WasteClassificationOverview = mapper.Map<NotificationApplication, WasteClassificationOverview>(overviewData.Notification),
                CompetentAuthority = (CompetentAuthority)overviewData.Notification.CompetentAuthority.Value,
                WasteRecovery = mapper.Map<WasteRecovery, WasteRecoveryOverview>(overviewData.WasteRecovery),
                WasteDisposal = mapper.Map<WasteDisposal, WasteDisposalOverview>(overviewData.WasteDisposal),
                NotificationNumber = overviewData.Notification.NotificationNumber,
                NotificationId = overviewData.Notification.Id,
                Journey = mapper.Map<NotificationApplication, Journey>(overviewData.Notification),
                RecoveryOperation = mapper.Map<NotificationApplication, RecoveryOperation>(overviewData.Notification),
                OrganisationsInvolved = mapper.Map<NotificationApplication, OrganisationsInvolved>(overviewData.Notification),
                Transportation = mapper.Map<NotificationApplication, Transportation>(overviewData.Notification),
                SubmitSummaryData = mapper.Map<NotificationApplication, SubmitSummaryData>(overviewData.Notification),
                WasteCodesOverview = mapper.Map<NotificationApplication, WasteCodesOverviewInfo>(overviewData.Notification),
                CanEditNotification = overviewData.NotificationAssessment.CanEditNotification,
                NotificationCharge = decimal.ToInt32(
                    notificationChargeCalculator.GetValue(pricingStructures, overviewData.Notification, shipmentInfo))
            };
        }
    }
}