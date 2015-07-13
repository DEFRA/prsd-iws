namespace EA.Iws.RequestHandlers.Shipment
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.Shipment;
    using DataAccess;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Shipment;

    internal class GetShipmentInfoForNotificationHandler : IRequestHandler<GetShipmentInfoForNotification, ShipmentData>
    {
        private readonly IwsContext context;
        private readonly IMap<NotificationApplication, ShipmentData> shipmentMapper;

        public GetShipmentInfoForNotificationHandler(IwsContext context,
            IMap<NotificationApplication, ShipmentData> shipmentMapper)
        {
            this.context = context;
            this.shipmentMapper = shipmentMapper;
        }

        public async Task<ShipmentData> HandleAsync(GetShipmentInfoForNotification message)
        {
            var notification = await context.GetNotificationApplication(message.NotificationId);

            return shipmentMapper.Map(notification);
        }
    }
}