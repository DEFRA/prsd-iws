namespace EA.Iws.RequestHandlers.Notification
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GetNotificationProgressInfoHandler :
        IRequestHandler<GetNotificationProgressInfo, NotificationProgressInfo>
    {
        private readonly IwsContext context;
        private readonly IMap<NotificationApplication, NotificationProgressInfo> mapper;

        public GetNotificationProgressInfoHandler(IwsContext context,
            IMap<NotificationApplication, NotificationProgressInfo> mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<NotificationProgressInfo> HandleAsync(GetNotificationProgressInfo message)
        {
            var notification = await context.GetNotificationApplication(message.NotificationId);

            return mapper.Map(notification);
        }
    }
}