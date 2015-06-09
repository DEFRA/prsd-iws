namespace EA.Iws.RequestHandlers.Notification
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Notification;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GetNotificationInfoHandler : IRequestHandler<GetNotificationInfo, NotificationInfo>
    {
        private readonly IwsContext db;

        public GetNotificationInfoHandler(IwsContext db)
        {
            this.db = db;
        }

        public async Task<NotificationInfo> HandleAsync(GetNotificationInfo message)
        {
            var notification = await db.NotificationApplications.SingleAsync(n => n.Id == message.NotificationId);

            return new NotificationInfo
            {
                NotificationId = message.NotificationId,
                CompetentAuthority = (CompetentAuthority)notification.CompetentAuthority.Value,
                NotificationNumber = notification.NotificationNumber,
                NotificationType =
                    notification.NotificationType == NotificationType.Disposal
                        ? Requests.Shared.NotificationType.Disposal
                        : Requests.Shared.NotificationType.Recovery
            };
        }
    }
}