namespace EA.Iws.RequestHandlers.NotificationMovements.Mappings
{
    using System;
    using System.Data;
    using System.Globalization;
    using Core.Movement.Bulk;
    using Prsd.Core.Mapper;

    public class ActualDateOfShipmentDataRowMap : IMap<DataRow, DateTime?>
    {
        public DateTime? Map(DataRow source)
        {
            DateTime? result = null;

            try
            {
                var data = source.ItemArray[(int)PrenotificationColumnIndex.ActualDateOfShipment].ToString();
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
