namespace EA.Iws.RequestHandlers.NotificationMovements.Mappings
{
    using System;
    using System.Data;
    using Prsd.Core.Mapper;

    public class ShipmentNumberDataRowMap : IMap<DataRow, int?>
    {
        private const int ColumnIndex = 1;

        public int? Map(DataRow source)
        {
            int? result = null;

            try
            {
                var val = source.ItemArray[ColumnIndex].ToString();
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
