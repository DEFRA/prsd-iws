namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core.Notification;
    using Core.Shipment;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Requests.Notification;

    internal class AmountsAndDatesInfoMap : IMap<NotificationApplication, AmountsAndDatesInfo>
    {
        private readonly IMap<NotificationApplication, ShipmentData> shipmentDataMap;

        public AmountsAndDatesInfoMap(
            IMap<NotificationApplication, ShipmentData> shipmentDataMap)
        {
            this.shipmentDataMap = shipmentDataMap;
        }

        public AmountsAndDatesInfo Map(NotificationApplication notification)
        {
            return new AmountsAndDatesInfo
            {
                NotificationId = notification.Id,
                NotificationType = notification.NotificationType == NotificationType.Disposal
                        ? Core.Shared.NotificationType.Disposal
                        : Core.Shared.NotificationType.Recovery,
                ShipmentData = shipmentDataMap.Map(notification)
            };
        }
    }
}
