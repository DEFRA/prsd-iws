namespace EA.Iws.DocumentGeneration.Formatters
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.NotificationApplication;

    public class PackagingTypesFormatter
    {
        public string PackagingTypesToCommaDelimitedString(IEnumerable<PackagingInfo> packagingTypes)
        {
            if (packagingTypes == null)
            {
                return string.Empty;
            }

            var packagingStrings = packagingTypes
                .OrderBy(c => c.PackagingType.Value)
                .Select(pt => pt.PackagingType != PackagingType.Other
                    ? pt.PackagingType.Value.ToString()
                    : pt.PackagingType.Value + "(" + pt.OtherDescription + ")");

            return string.Join(", ", packagingStrings);
        }
    }
}
