namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.Shipment;
    using Domain.NotificationApplication.Shipment;
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
                    NotificaitonId = source.NotificationId,
                    HasHistoryData = true,
                    NumberOfShipments = source.NumberOfShipments,
                    DateChanged = source.DateChanged
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
