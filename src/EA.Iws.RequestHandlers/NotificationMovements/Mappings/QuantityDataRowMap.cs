namespace EA.Iws.RequestHandlers.NotificationMovements.Mappings
{
    using System;
    using System.Data;
    using Prsd.Core.Mapper;

    public class QuantityDataRowMap : IMap<DataRow, decimal?>
    {
        private const int ColumnIndex = 2;

        public decimal? Map(DataRow source)
        {
            decimal? result = null;

            try
            {
                var val = source.ItemArray[ColumnIndex].ToString();
                decimal parsed;

                if (decimal.TryParse(val, out parsed))
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
