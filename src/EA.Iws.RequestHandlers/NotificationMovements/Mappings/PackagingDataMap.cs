namespace EA.Iws.RequestHandlers.NotificationMovements.Mappings
{
    using System.Collections.Generic;
    using Core.PackagingType;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using PackagingTypeEnum = Core.PackagingType.PackagingType;

    internal class PackagingDataMap : IMap<List<PackagingInfo>, PackagingData>
    {
        public PackagingData Map(List<PackagingInfo> source)
        {
            var data = new PackagingData();

            foreach (var item in source)
            {
                data.PackagingTypes.Add((PackagingTypeEnum)item.PackagingType.Value);

                if (item.PackagingType.Value == (int)PackagingTypeEnum.Other)
                {
                    data.OtherDescription = item.OtherDescription;
                }
            }

            return data;
        }
    }
}