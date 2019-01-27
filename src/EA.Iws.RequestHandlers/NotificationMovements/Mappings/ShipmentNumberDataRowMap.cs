namespace EA.Iws.RequestHandlers.NotificationMovements.Mappings
{
    using System;
    using System.Data;
    using Core.Movement.Bulk;
    using Prsd.Core.Mapper;

    public class ShipmentNumberDataRowMap : IMap<DataRow, int?>
    {
        public int? Map(DataRow source)
        {
            int? result = null;

            try
            {
                var val = source.ItemArray[(int)PrenotificationColumnIndex.ShipmentNumber].ToString();
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
