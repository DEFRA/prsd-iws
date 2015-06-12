namespace EA.Iws.RequestHandlers.Notification
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GetSpecialHandingForNotificationHandler :
        IRequestHandler<GetSpecialHandingForNotification, SpecialHandlingData>
    {
        private readonly IwsContext context;
        private readonly IMap<NotificationApplication, SpecialHandlingData> specialHandlingMapper;

        public GetSpecialHandingForNotificationHandler(IwsContext context, IMap<NotificationApplication, SpecialHandlingData> specialHandlingMapper)
        {
            this.context = context;
            this.specialHandlingMapper = specialHandlingMapper;
        }

        public async Task<SpecialHandlingData> HandleAsync(GetSpecialHandingForNotification message)
        {
            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == message.NotificationId);

            return specialHandlingMapper.Map(notification);
        }
    }
}