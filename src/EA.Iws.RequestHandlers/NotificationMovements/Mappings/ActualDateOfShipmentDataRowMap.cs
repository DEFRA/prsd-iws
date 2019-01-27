namespace EA.Iws.RequestHandlers.NotificationMovements.Mappings
{
    using System;
    using System.Data;
    using System.Globalization;
    using Prsd.Core.Mapper;

    public class ActualDateOfShipmentDataRowMap : IMap<DataRow, DateTime?>
    {
        private const int ColumnIndex = 5;

        public DateTime? Map(DataRow source)
        {
            DateTime? result = null;

            try
            {
                var data = source.Field<DateTime>(ColumnIndex);
                result = data;
            }
            catch
            {
                // ignored
            }

            return result;
        }
    }
}
