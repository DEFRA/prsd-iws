namespace EA.Iws.RequestHandlers.WasteComponentType
{
    using EA.Iws.Core.WasteComponentType;
    using EA.Iws.DataAccess;
    using EA.Iws.Domain.NotificationApplication;
    using EA.Iws.Requests.WasteComponentType;
    using EA.Prsd.Core.Mapper;
    using EA.Prsd.Core.Mediator;
    using System.Threading.Tasks;

    internal class GetWasteComponentInfoForNotificationHandler : IRequestHandler<GetWasteComponentInfoForNotification, WasteComponentData>
    {
        private readonly IwsContext context;
        private readonly IMap<NotificationApplication, WasteComponentData> wasteComponentMapper;

        public GetWasteComponentInfoForNotificationHandler(IwsContext context, IMap<NotificationApplication, WasteComponentData> wasteComponentMapper)
        {
            this.context = context;
            this.wasteComponentMapper = wasteComponentMapper;
        }

        public async Task<WasteComponentData> HandleAsync(GetWasteComponentInfoForNotification message)
        {
            var notification = await context.GetNotificationApplication(message.NotificationId);

            return wasteComponentMapper.Map(notification);
        }
    }
}
