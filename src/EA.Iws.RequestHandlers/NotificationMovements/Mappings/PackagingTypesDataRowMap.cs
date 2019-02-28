namespace EA.Iws.RequestHandlers.NotificationMovements.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Linq;
    using Core.Movement.BulkPrenotification;
    using Core.PackagingType;
    using Prsd.Core.Mapper;

    public class PackagingTypesDataRowMap : IMap<DataRow, IList<PackagingType>>
    {
        private readonly char[] separator = { ';' };

        private Dictionary<string, PackagingType> displayNameMapping;

        public PackagingTypesDataRowMap()
        {
            GetEnumDisplayNames();
        }

        public IList<PackagingType> Map(DataRow source)
        {
            var result = new List<PackagingType>();

            try
            {
                var data = source.ItemArray[(int)PrenotificationColumnIndex.PackagingType].ToString();
                var packagingTypes = data.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                foreach (var packaging in packagingTypes)
                {
                    PackagingType parsed;

                    if (Enum.TryParse(packaging, true, out parsed) &&
                        Enum.IsDefined(typeof(PackagingType), parsed))
                    {
                        result.Add(parsed);
                    }
                    else if (displayNameMapping.ContainsKey(packaging.ToLower()))
                    {
                        result.Add(displayNameMapping[packaging.ToLower()]);
                    }
                }

                // Clear the list if not ALL of the data is valid.
                if (result.Count != packagingTypes.Length)
                {
                    result.Clear();
                }
            }
            catch
            {
                //ignored
            }

            return result;
        }

        private void GetEnumDisplayNames()
        {
            displayNameMapping = new Dictionary<string, PackagingType>();
            var type = typeof(PackagingType);

            Enum.GetNames(type).ToList().ForEach(name =>
            {
                var member = type.GetMember(name).First();
                var displayAttribute = (DisplayAttribute)member.GetCustomAttributes(typeof(DisplayAttribute), false).First();
                displayNameMapping.Add(displayAttribute.Name.ToLower(), (PackagingType)Enum.Parse(type, name));
            });
        }
    }
}
