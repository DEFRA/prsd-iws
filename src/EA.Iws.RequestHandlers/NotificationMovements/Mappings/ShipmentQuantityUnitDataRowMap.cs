namespace EA.Iws.RequestHandlers.NotificationMovements.Mappings
{
    using System;
    using System.Data;
    using Core.Movement.BulkPrenotification;
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
                var data = source.ItemArray[(int)PrenotificationColumnIndex.Unit].ToString();

                // Small 'hack' when this is supplied as the unit.
                data = string.Equals(data, "m3", StringComparison.InvariantCultureIgnoreCase) ? "CubicMetres" : data;

                if (Enum.TryParse(data, true, out parsed) &&
                    Enum.IsDefined(typeof(ShipmentQuantityUnits), parsed))
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
