namespace EA.Iws.RequestHandlers.WasteCodes
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.WasteCodes;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.WasteCodes;

    internal class GetAllWasteCodesForNotificationHandler :
        IRequestHandler<GetAllWasteCodesForNotification, WasteCodeData[]>
    {
        private readonly IwsContext context;
        private readonly IMap<IEnumerable<WasteCodeInfo>, WasteCodeData[]> mapper;

        public GetAllWasteCodesForNotificationHandler(IwsContext context,
            IMap<IEnumerable<WasteCodeInfo>, WasteCodeData[]> mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<WasteCodeData[]> HandleAsync(GetAllWasteCodesForNotification message)
        {
            var notification = await context.GetNotificationApplication(message.NotificationId);

            return mapper.Map(notification.WasteCodes);
        }
    }
}