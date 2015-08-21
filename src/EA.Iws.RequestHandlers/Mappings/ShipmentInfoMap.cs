namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.IntendedShipments;
    using Core.Shared;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;

    internal class ShipmentInfoMap : IMap<NotificationApplication, IntendedShipmentData>
    {
        private readonly IMap<Domain.ShipmentQuantityUnits, ShipmentQuantityUnits> shipmentQuantityUnitsMapper;

        public ShipmentInfoMap(IMap<Domain.ShipmentQuantityUnits, ShipmentQuantityUnits> shipmentQuantityUnitsMapper)
        {
            this.shipmentQuantityUnitsMapper = shipmentQuantityUnitsMapper;
        }

        public IntendedShipmentData Map(NotificationApplication source)
        {
            IntendedShipmentData data;
            if (source.HasShipmentInfo)
            {
                data = new IntendedShipmentData
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
                data = new IntendedShipmentData
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