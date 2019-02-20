namespace EA.Iws.RequestHandlers.NotificationMovements.Mappings.ReceiptRecoveryMappings
{
    using System;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Shared;
    using Prsd.Core.Mapper;

    public class ShipmentQuantityUnitDataRowMap : IMap<ReceiptRecoveryDataRow, ShipmentQuantityUnits?>
    {
        public ShipmentQuantityUnits? Map(ReceiptRecoveryDataRow source)
        {
            ShipmentQuantityUnits? result = null;

            try
            {
                ShipmentQuantityUnits parsed;
                var data = source.DataRow.ItemArray[(int)ReceiptRecoveryColumnIndex.Unit].ToString();

                // Small 'hack' when this is supplied as the unit.
                data = data == "m3" ? "CubicMetres" : data;

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
