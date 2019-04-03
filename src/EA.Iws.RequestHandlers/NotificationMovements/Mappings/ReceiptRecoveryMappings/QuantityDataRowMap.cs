namespace EA.Iws.RequestHandlers.NotificationMovements.Mappings.ReceiptRecoveryMappings
{
    using System;
    using System.Data;
    using System.Globalization;
    using Core.Movement.BulkReceiptRecovery;
    using Prsd.Core.Mapper;

    public class QuantityDataRowMap : IMap<ReceiptRecoveryDataRow, decimal?>
    {
        public decimal? Map(ReceiptRecoveryDataRow source)
        {
            decimal? result = null;

            try
            {
                var val = source.DataRow.ItemArray[(int)ReceiptRecoveryColumnIndex.Quantity].ToString();
                decimal parsed;

                if (decimal.TryParse(val, NumberStyles.AllowDecimalPoint, new NumberFormatInfo(), out parsed))
                {
                    if (NumberIsValid(parsed) && parsed > 0)
                    {
                        result = parsed;
                    }
                }
            }
            catch
            {
                // ignored
            }

            return result;
        }

        private static bool NumberIsValid(decimal number)
        {
            var maxNumber = (long)Math.Pow(10, 18) - 1;
            var minNumber = 0 - maxNumber;

            return number > minNumber && number < maxNumber;
        }
    }
}
