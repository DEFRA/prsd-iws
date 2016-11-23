namespace EA.Iws.RequestHandlers.Mappings.ImportNotification
{
    using Core.Shipment;
    using Domain.ImportNotification;
    using Prsd.Core.Mapper;

    internal class ShipmentNumberHistoryMap : IMap<ShipmentNumberHistory, ShipmentNumberHistoryData>
    {
        public ShipmentNumberHistoryData Map(ShipmentNumberHistory source)
        {
            ShipmentNumberHistoryData data;
            if (source != null)
            {
                data = new ShipmentNumberHistoryData
                {
                    NotificaitonId = source.ImportNotificationId,
                    HasHistoryData = true,
                    NumberOfShipments = source.NumberOfShipments,
                    DateChanged = source.DateChanged
                };
            }
            else
            {
                data = new ShipmentNumberHistoryData
                {
                    HasHistoryData = false
                };
            }
            return data;
        }
    }
}
