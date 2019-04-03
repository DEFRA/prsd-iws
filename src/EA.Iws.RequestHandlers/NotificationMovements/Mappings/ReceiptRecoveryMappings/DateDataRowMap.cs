namespace EA.Iws.RequestHandlers.NotificationMovements.Mappings.ReceiptRecoveryMappings
{
    using System;
    using System.Data;
    using System.Globalization;
    using Core.Movement.BulkReceiptRecovery;
    using Prsd.Core.Mapper;

    public class ActualDateOfShipmentDataRowMap : IMap<ReceiptRecoveryDataRow, DateTime?>
    {
        private readonly string[] formats = { "dd/MM/yyyy", "d/MM/yyyy", "d/M/yyyy", "dd/M/yyyy" };

        public DateTime? Map(ReceiptRecoveryDataRow source)
        {
            DateTime? result = null;

            try
            {
                int index = source.IsReceivedDate ? (int)ReceiptRecoveryColumnIndex.Received : (int)ReceiptRecoveryColumnIndex.RecoveredDisposed;
                var data = source.DataRow.ItemArray[index].ToString();
                data = data.Trim().Split(' ')[0];
                DateTime parsed;

                foreach (var format in formats)
                {
                    if (DateTime.TryParseExact(data, format, CultureInfo.InvariantCulture, DateTimeStyles.None,
                        out parsed))
                    {
                        result = parsed;
                        break;
                    }
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
