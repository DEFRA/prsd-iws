namespace EA.Iws.RequestHandlers.NotificationMovements.Mappings
{
    using System;
    using System.Data;
    using Core.Movement.Bulk;
    using Prsd.Core.Mapper;

    public class QuantityDataRowMap : IMap<DataRow, decimal?>
    {
        public decimal? Map(DataRow source)
        {
            decimal? result = null;

            try
            {
                var val = source.ItemArray[(int)PrenotificationColumnIndex.Quantity].ToString();
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
