namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core.IntendedShipments;
    using Core.Notification;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using RequestHandlers.Notification;
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
                    context.ShipmentInfos.Single(si => si.NotificationId == notification.Id))
            };
        }
    }
}
