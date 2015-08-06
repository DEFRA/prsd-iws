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
        private readonly IMap<NotificationApplication, NotificationApplicationCompletionProgress> completionProgressMap;
        private readonly IMap<NotificationApplication, ShipmentData> shipmentDataMap;

        public AmountsAndDatesInfoMap(
            IMap<NotificationApplication, NotificationApplicationCompletionProgress> completionProgressMap,
            IMap<NotificationApplication, ShipmentData> shipmentDataMap)
        {
            this.completionProgressMap = completionProgressMap;
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
                Progress = completionProgressMap.Map(notification),
                ShipmentData = shipmentDataMap.Map(notification)
            };
        }
    }
}
