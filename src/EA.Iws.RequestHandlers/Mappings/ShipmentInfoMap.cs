namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.Shipment;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Requests.Shipment;

    internal class ShipmentInfoMap : IMap<NotificationApplication, ShipmentData>
    {
        private readonly IMap<Domain.ShipmentQuantityUnits, ShipmentQuantityUnits> shipmentQuantityUnitsMapper;

        public ShipmentInfoMap(IMap<Domain.ShipmentQuantityUnits, ShipmentQuantityUnits> shipmentQuantityUnitsMapper)
        {
            this.shipmentQuantityUnitsMapper = shipmentQuantityUnitsMapper;
        }

        public ShipmentData Map(NotificationApplication source)
        {
            ShipmentData data;
            if (source.HasShipmentInfo)
            {
                data = new ShipmentData
                {
                    NotificationId = source.Id,
                    HasShipmentData = true,
                    IsPreconsentedRecoveryFacility = source.IsPreconsentedRecoveryFacility.GetValueOrDefault(),
                    FirstDate = source.ShipmentInfo.FirstDate,
                    LastDate = source.ShipmentInfo.LastDate,
                    NumberOfShipments = source.ShipmentInfo.NumberOfShipments,
                    Quantity = source.ShipmentInfo.Quantity,
                    Units = shipmentQuantityUnitsMapper.Map(source.ShipmentInfo.Units)
                };
            }
            else
            {
                data = new ShipmentData
                {
                    NotificationId = source.Id,
                    HasShipmentData = false,
                    IsPreconsentedRecoveryFacility = source.IsPreconsentedRecoveryFacility.GetValueOrDefault(),
                };
            }
            return data;
        }
    }
}