namespace EA.Iws.RequestHandlers.NotificationMovements.Mappings
{
    using System;
    using System.Data;
    using Core.Movement.Bulk;
    using Core.Shared;
    using Prsd.Core.Mapper;

    public class ShipmentQuantityUnitDataRowMap : IMap<DataRow, ShipmentQuantityUnits?>
    {

        public ShipmentQuantityUnits? Map(DataRow source)
        {
            ShipmentQuantityUnits? result = null;

            try
            {
                ShipmentQuantityUnits parsed;
                var data = source.ItemArray[(int)PrenotificationColumnIndex.Quantity].ToString();

                if (Enum.TryParse(data, out parsed))
                {
                    result = parsed;
                }
            }
            catch
            {
                // ignored
            }

            return result;
        }
    }
}
