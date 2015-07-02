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

        public GetNotificationInfoHandler(IwsContext db,
            IMap<NotificationApplication, NotificationInfo> notificationInfoMap)
        {
            this.db = db;
            this.notificationInfoMap = notificationInfoMap;
        }

        public async Task<NotificationInfo> HandleAsync(GetNotificationInfo message)
        {
            var notification = await db.NotificationApplications.SingleAsync(n => n.Id == message.NotificationId);

            return notificationInfoMap.Map(notification);
        }
    }
}