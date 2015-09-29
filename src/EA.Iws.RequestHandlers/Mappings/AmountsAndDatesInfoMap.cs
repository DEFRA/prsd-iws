namespace EA.Iws.RequestHandlers.Mappings
{
    using System.Linq;
    using Core.IntendedShipments;
    using DataAccess;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Shipment;
    using Prsd.Core.Mapper;
    using Requests.Notification;

    internal class AmountsAndDatesInfoMap : IMap<NotificationApplication, AmountsAndDatesInfo>
    {
        private readonly IMap<ShipmentInfo, IntendedShipmentData> shipmentDataMap;
        private readonly IwsContext context;

        public AmountsAndDatesInfoMap(IwsContext context,
            IMap<ShipmentInfo, IntendedShipmentData> shipmentDataMap)
        {
            this.shipmentDataMap = shipmentDataMap;
            this.context = context;
        }

        public AmountsAndDatesInfo Map(NotificationApplication notification)
        {
            return new AmountsAndDatesInfo
            {
                NotificationId = notification.Id,
                NotificationType = notification.NotificationType == NotificationType.Disposal
                        ? Core.Shared.NotificationType.Disposal
                        : Core.Shared.NotificationType.Recovery,
                IntendedShipmentData = shipmentDataMap.Map(
                    context.ShipmentInfos.SingleOrDefault(si => si.NotificationId == notification.Id))
            };
        }
    }
}
