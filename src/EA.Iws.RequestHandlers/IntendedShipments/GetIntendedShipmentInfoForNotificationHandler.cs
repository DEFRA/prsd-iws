namespace EA.Iws.RequestHandlers.IntendedShipments
{
    using System.Threading.Tasks;
    using Core.IntendedShipments;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.IntendedShipments;

    internal class GetIntendedShipmentInfoForNotificationHandler : IRequestHandler<GetIntendedShipmentInfoForNotification, IntendedShipmentData>
    {
        private readonly IwsContext context;
        private readonly IMap<NotificationApplication, IntendedShipmentData> shipmentMapper;

        public GetIntendedShipmentInfoForNotificationHandler(IwsContext context,
            IMap<NotificationApplication, IntendedShipmentData> shipmentMapper)
        {
            this.context = context;
            this.shipmentMapper = shipmentMapper;
        }

        public async Task<IntendedShipmentData> HandleAsync(GetIntendedShipmentInfoForNotification message)
        {
            var notification = await context.GetNotificationApplication(message.NotificationId);

            return shipmentMapper.Map(notification);
        }
    }
}