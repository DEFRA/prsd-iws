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

    internal class GetNotificationInfoHandler : IRequestHandler<GetNotificationInfo, NotificationInfo>
    {
        private readonly IwsContext db;
        private readonly IMap<NotificationApplication, NotificationInfo> notificationInfoMap;
        private readonly NotificationChargeCalculator notificationChargeCalculator;
        private readonly IShipmentInfoRepository shipmentInfoRepository;

        public GetNotificationInfoHandler(IwsContext db,
            IMap<NotificationApplication, NotificationInfo> notificationInfoMap,
            NotificationChargeCalculator notificationChargeCalculator,
            IShipmentInfoRepository shipmentInfoRepository)
        {
            this.db = db;
            this.notificationInfoMap = notificationInfoMap;
            this.notificationChargeCalculator = notificationChargeCalculator;
            this.shipmentInfoRepository = shipmentInfoRepository;
        }

        public async Task<NotificationInfo> HandleAsync(GetNotificationInfo message)
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