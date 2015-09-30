namespace EA.Iws.RequestHandlers.Notification
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Shipment;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using Requests.Notification.Overview;

    internal class GetNotificationOverviewHandler : IRequestHandler<GetNotificationOverview, NotificationOverview>
    {
        private readonly IwsContext db;
        private readonly IMap<NotificationApplication, NotificationOverview> notificationInfoMap;
        private readonly NotificationChargeCalculator notificationChargeCalculator;
        private readonly IShipmentInfoRepository shipmentInfoRepository;

        public GetNotificationOverviewHandler(IwsContext db,
            IMap<NotificationApplication, NotificationOverview> notificationInfoMap,
            NotificationChargeCalculator notificationChargeCalculator,
            IShipmentInfoRepository shipmentInfoRepository)
        {
            this.db = db;
            this.notificationInfoMap = notificationInfoMap;
            this.notificationChargeCalculator = notificationChargeCalculator;
            this.shipmentInfoRepository = shipmentInfoRepository;
        }

        public async Task<NotificationOverview> HandleAsync(GetNotificationOverview message)
        {
            var notification = await db.GetNotificationApplication(message.NotificationId);
            var pricingStructures = await db.PricingStructures.ToArrayAsync();
            var shipmentInfo = await shipmentInfoRepository.GetByNotificationId(message.NotificationId);

            var notificationInfo = notificationInfoMap.Map(notification);
            notificationInfo.NotificationCharge = decimal.ToInt32(
                notificationChargeCalculator.GetValue(pricingStructures, notification, shipmentInfo));

            return notificationInfo;
        }
    }
}