namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.IntendedShipments;
    using Domain.NotificationApplication.Shipment;
    using Prsd.Core.Mapper;

    internal class ShipmentInfoMap : IMap<ShipmentInfo, IntendedShipmentData>
    {
        public IntendedShipmentData Map(ShipmentInfo source)
        {
            IntendedShipmentData data;
            if (source != null)
            {
                data = new IntendedShipmentData
                {
                    NotificationId = source.NotificationId,
                    HasShipmentData = true,
                    FirstDate = source.ShipmentPeriod.FirstDate,
                    LastDate = source.ShipmentPeriod.LastDate,
                    NumberOfShipments = source.NumberOfShipments,
                    Quantity = source.Quantity,
                    Units = source.Units
                };
            }
            else
            {
                data = new IntendedShipmentData
                {
                    HasShipmentData = false
                };
            }
            return data;
        }
    }
}