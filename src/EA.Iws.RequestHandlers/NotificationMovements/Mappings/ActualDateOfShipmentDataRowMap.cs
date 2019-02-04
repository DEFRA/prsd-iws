namespace EA.Iws.RequestHandlers.NotificationMovements.Mappings
{
    using System;
    using System.Data;
    using System.Globalization;
    using Core.Movement.Bulk;
    using Prsd.Core.Mapper;

    public class ActualDateOfShipmentDataRowMap : IMap<DataRow, DateTime?>
    {
        private readonly string[] formats = { "dd/MM/yyyy", "d/MM/yyyy", "d/M/yyyy", "dd/M/yyyy" };

        public DateTime? Map(DataRow source)
        {
            DateTime? result = null;

            try
            {
                var data = source.ItemArray[(int)PrenotificationColumnIndex.ActualDateOfShipment].ToString();
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
