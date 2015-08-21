namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core.IntendedShipments;
    using Core.Notification;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Requests.Notification;

    internal class AmountsAndDatesInfoMap : IMap<NotificationApplication, AmountsAndDatesInfo>
    {
        private readonly IMap<NotificationApplication, IntendedShipmentData> shipmentDataMap;

        public AmountsAndDatesInfoMap(
            IMap<NotificationApplication, IntendedShipmentData> shipmentDataMap)
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
                IntendedShipmentData = shipmentDataMap.Map(notification)
            };
        }
    }
}
