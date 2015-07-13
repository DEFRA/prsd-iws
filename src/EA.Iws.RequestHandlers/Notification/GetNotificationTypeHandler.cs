namespace EA.Iws.RequestHandlers.Notification
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GetNotificationBasicInfoHandler : IRequestHandler<GetNotificationBasicInfo, NotificationBasicInfo>
    {
        private readonly IwsContext context;
        private readonly IMap<NotificationApplication, NotificationBasicInfo> mapper;

        public GetNotificationBasicInfoHandler(IwsContext context,
            IMap<NotificationApplication, NotificationBasicInfo> mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<NotificationBasicInfo> HandleAsync(GetNotificationBasicInfo message)
        {
            var notification = await context.GetNotificationApplication(message.NotificationId);

            return mapper.Map(notification);
        }
    }
}