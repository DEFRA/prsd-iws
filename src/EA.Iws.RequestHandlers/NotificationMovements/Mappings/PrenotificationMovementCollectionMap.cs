namespace EA.Iws.RequestHandlers.NotificationMovements.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Core.Movement.Bulk;
    using Core.PackagingType;
    using Core.Shared;
    using Prsd.Core.Mapper;

    public class PrenotificationMovementCollectionMap : IMap<DataTable, List<PrenotificationMovement>>
    {
        private readonly IMap<DataRow, string> notificationNumberMapper;
        private readonly IMap<DataRow, int?> shipmentNumberMapper;
        private readonly IMap<DataRow, decimal?> quantityMapper;
        private readonly IMap<DataRow, ShipmentQuantityUnits?> unitsMapper;
        private readonly IMap<DataRow, IList<PackagingType>> packagingMapper;
        private readonly IMap<DataRow, DateTime?> actualDateOfShipmentMapper;

        public PrenotificationMovementCollectionMap(IMap<DataRow, string> notificationNumberMapper,
            IMap<DataRow, int?> shipmentNumberMapper,
            IMap<DataRow, decimal?> quantityMapper,
            IMap<DataRow, ShipmentQuantityUnits?> unitsMapper,
            IMap<DataRow, IList<PackagingType>> packagingMapper,
            IMap<DataRow, DateTime?> actualDateOfShipmentMapper)
        {
            this.notificationNumberMapper = notificationNumberMapper;
            this.shipmentNumberMapper = shipmentNumberMapper;
            this.quantityMapper = quantityMapper;
            this.unitsMapper = unitsMapper;
            this.packagingMapper = packagingMapper;
            this.actualDateOfShipmentMapper = actualDateOfShipmentMapper;
        }

        public List<PrenotificationMovement> Map(DataTable source)
        {
            return source.AsEnumerable().Select(data =>
                new PrenotificationMovement()
                {
                    NotificationNumber = notificationNumberMapper.Map(data),
                    ShipmentNumber = shipmentNumberMapper.Map(data),
                    Quantity = quantityMapper.Map(data),
                    Unit = unitsMapper.Map(data),
                    PackagingTypes = packagingMapper.Map(data),
                    ActualDateOfShipment = actualDateOfShipmentMapper.Map(data)
                }).ToList();
        }
    }
}
