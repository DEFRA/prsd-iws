namespace EA.Iws.RequestHandlers.NotificationMovements.Mappings.ReceiptRecoveryMappings
{
    using Core.Movement.BulkReceiptRecovery;
    using Prsd.Core.Mapper;

    public class ShipmentNumberDataRowMap : IMap<ReceiptRecoveryDataRow, int?>
    {
        public int? Map(ReceiptRecoveryDataRow source)
        {
            int? result = null;

            try
            {
                var val = source.DataRow.ItemArray[(int)ReceiptRecoveryColumnIndex.ShipmentNumber].ToString();
                int parsed;

                if (int.TryParse(val, out parsed))
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
