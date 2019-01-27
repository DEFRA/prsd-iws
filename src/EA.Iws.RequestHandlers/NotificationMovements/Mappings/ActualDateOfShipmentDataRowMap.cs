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
                var data = source.ItemArray[ColumnIndex].ToString();
                data = data.Trim().Split(' ')[0];
                DateTime parsed;

                if (DateTime.TryParseExact(data, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out parsed))
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
