namespace EA.Iws.RequestHandlers.Mappings
{
    using System.Linq;
    using Core.IntendedShipments;
    using Core.Notification.Overview;
    using DataAccess;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Shipment;
    using Prsd.Core.Mapper;

    internal class AmountsAndDatesInfoMap : IMap<NotificationApplication, ShipmentOverview>
    {
        private readonly IMap<ShipmentInfo, IntendedShipmentData> shipmentDataMap;
        private readonly IwsContext context;

        public AmountsAndDatesInfoMap(IwsContext context,
            IMap<ShipmentInfo, IntendedShipmentData> shipmentDataMap)
        {
            this.shipmentDataMap = shipmentDataMap;
            this.context = context;
        }

        public ShipmentOverview Map(NotificationApplication notification)
        {
            return new ShipmentOverview
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
