namespace EA.Iws.RequestHandlers.Notification
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.Notification;
    using DataAccess;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GetNotificationInfoHandler : IRequestHandler<GetNotificationInfo, NotificationInfo>
    {
        private readonly IwsContext db;
        private readonly IMap<NotificationApplication, NotificationApplicationCompletionProgress> completionProgressMapper;

        public GetNotificationInfoHandler(IwsContext db, IMap<NotificationApplication, NotificationApplicationCompletionProgress> completionProgressMapper)
        {
            this.db = db;
            this.completionProgressMapper = completionProgressMapper;
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
                        ? Core.Shared.NotificationType.Disposal
                        : Core.Shared.NotificationType.Recovery,
                Progress = completionProgressMapper.Map(notification)
            };
        }
    }
}