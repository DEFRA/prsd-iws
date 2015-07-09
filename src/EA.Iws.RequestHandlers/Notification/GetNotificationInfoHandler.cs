namespace EA.Iws.RequestHandlers.Notification
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GetNotificationInfoHandler : IRequestHandler<GetNotificationInfo, NotificationInfo>
    {
        private readonly IwsContext db;
        private readonly IMap<NotificationApplication, NotificationInfo> notificationInfoMap;
        private readonly INotificationChargeCalculator notificationChargeCalculator;

        public GetNotificationInfoHandler(IwsContext db,
            IMap<NotificationApplication, NotificationInfo> notificationInfoMap,
            INotificationChargeCalculator notificationChargeCalculator)
        {
            this.db = db;
            this.notificationInfoMap = notificationInfoMap;
            this.notificationChargeCalculator = notificationChargeCalculator;
        }

        public async Task<NotificationInfo> HandleAsync(GetNotificationInfo message)
        {
            var notification = await db.NotificationApplications.SingleAsync(n => n.Id == message.NotificationId);

            var notificationInfo = notificationInfoMap.Map(notification);
            notificationInfo.NotificationCharge = decimal.ToInt32(await notificationChargeCalculator.GetValue(message.NotificationId));

            return notificationInfo;
        }
    }
}