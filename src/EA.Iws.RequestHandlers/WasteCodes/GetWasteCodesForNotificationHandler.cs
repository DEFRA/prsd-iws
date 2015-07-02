namespace EA.Iws.RequestHandlers.WasteCodes
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.WasteCodes;
    using DataAccess;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.WasteCodes;

    internal class GetWasteCodesForNotificationHandler :
        IRequestHandler<GetWasteCodesForNotification, WasteCodeData[]>
    {
        private readonly IwsContext context;
        private readonly IMapWithParameter<NotificationApplication, CodeType, WasteCodeData[]> mapper;

        public GetWasteCodesForNotificationHandler(IwsContext context,
            IMapWithParameter<NotificationApplication, CodeType, WasteCodeData[]> mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<WasteCodeData[]> HandleAsync(GetWasteCodesForNotification message)
        {
            var notification =
                await
                    context.NotificationApplications.SingleAsync(n => n.Id == message.NotificationId);

            return mapper.Map(notification, message.CodeType);
        }
    }
}