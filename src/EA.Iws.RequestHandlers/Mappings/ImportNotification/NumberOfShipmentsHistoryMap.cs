namespace EA.Iws.RequestHandlers.Mappings.ImportNotification
{
    using Core.Shipment;
    using Domain.ImportNotification;
    using Prsd.Core.Mapper;

    internal class NumberOfShipmentsHistoryMap : IMap<NumberOfShipmentsHistory, NumberOfShipmentsHistoryData>
    {
        public NumberOfShipmentsHistoryData Map(NumberOfShipmentsHistory source)
        {
            NumberOfShipmentsHistoryData data;
            if (source != null)
            {
                data = new NumberOfShipmentsHistoryData
                {
                    NotificaitonId = source.ImportNotificationId,
                    HasHistoryData = true,
                    NumberOfShipments = source.NumberOfShipments,
                    DateChanged = source.DateChanged.DateTime
                };
            }
            else
            {
                data = new NumberOfShipmentsHistoryData
                {
                    HasHistoryData = false
                };
            }
            return data;
        }
    }
}
