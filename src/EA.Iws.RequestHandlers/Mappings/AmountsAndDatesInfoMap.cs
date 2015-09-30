namespace EA.Iws.RequestHandlers.Mappings
{
    using System.Linq;
    using Core.IntendedShipments;
    using DataAccess;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Shipment;
    using Prsd.Core.Mapper;
    using Requests.Notification.Overview;

    internal class AmountsAndDatesInfoMap : IMap<NotificationApplication, AmountsAndDates>
    {
        private readonly IMap<ShipmentInfo, IntendedShipmentData> shipmentDataMap;
        private readonly IwsContext context;

        public AmountsAndDatesInfoMap(IwsContext context,
            IMap<ShipmentInfo, IntendedShipmentData> shipmentDataMap)
        {
            this.shipmentDataMap = shipmentDataMap;
            this.context = context;
        }

        public AmountsAndDates Map(NotificationApplication notification)
        {
            return new AmountsAndDates
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
