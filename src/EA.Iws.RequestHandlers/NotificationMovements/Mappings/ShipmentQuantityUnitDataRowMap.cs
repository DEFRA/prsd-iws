namespace EA.Iws.RequestHandlers.NotificationMovements.Mappings
{
    using System;
    using System.Data;
    using Core.Shared;
    using Prsd.Core.Mapper;

    public class ShipmentQuantityUnitDataRowMap : IMap<DataRow, ShipmentQuantityUnits?>
    {
        private const int ColumnIndex = 3;

        public ShipmentQuantityUnits? Map(DataRow source)
        {
            ShipmentQuantityUnits? result = null;
            ShipmentQuantityUnits parsed;
            var data = source.Field<string>(ColumnIndex);
            
            if (Enum.TryParse(data, out parsed))
            {
                result = parsed;
            }

            return result;
        }
    }
}
