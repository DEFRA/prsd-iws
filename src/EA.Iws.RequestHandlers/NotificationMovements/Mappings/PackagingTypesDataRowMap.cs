namespace EA.Iws.RequestHandlers.NotificationMovements.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Core.PackagingType;
    using Prsd.Core.Mapper;

    public class PackagingTypesDataRowMap : IMap<DataRow, IList<PackagingType>>
    {
        private const int ColumnIndex = 4;
        private readonly char[] separator = { ';' };

        public IList<PackagingType> Map(DataRow source)
        {
            var result = new List<PackagingType>();

            try
            {
                var data = source.ItemArray[ColumnIndex].ToString();
                var packagingTypes = data.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                foreach (var packaging in packagingTypes)
                {
                    PackagingType parsed;

                    if (Enum.TryParse(packaging, out parsed))
                    {
                        result.Add(parsed);
                    }
                }
            }
            catch
            {
                //ignored
            }

            return result;
        }
    }
}
