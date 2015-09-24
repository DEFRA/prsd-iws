namespace EA.Iws.RequestHandlers.IntendedShipments
{
    using System.Threading.Tasks;
    using Core.IntendedShipments;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using RequestHandlers.Notification;
    using Requests.IntendedShipments;

    internal class GetIntendedShipmentInfoForNotificationHandler : IRequestHandler<GetIntendedShipmentInfoForNotification, IntendedShipmentData>
    {
        private readonly IwsContext context;
        private readonly IMap<ShipmentInfo, IntendedShipmentData> shipmentMapper;

        public GetIntendedShipmentInfoForNotificationHandler(IwsContext context,
            IMap<ShipmentInfo, IntendedShipmentData> shipmentMapper)
        {
            this.context = context;
            this.shipmentMapper = shipmentMapper;
        }

        public async Task<IntendedShipmentData> HandleAsync(GetIntendedShipmentInfoForNotification message)
        {
            var notification = await context.GetNotificationApplication(message.NotificationId);
            var shipmentInfo = await context.GetShipmentInfoAsync(message.NotificationId);
           
            var data = shipmentMapper.Map(shipmentInfo);
            data.NotificationId = notification.Id;
            data.IsPreconsentedRecoveryFacility = notification.IsPreconsentedRecoveryFacility.GetValueOrDefault();
            
            return data;
        }
    }
}