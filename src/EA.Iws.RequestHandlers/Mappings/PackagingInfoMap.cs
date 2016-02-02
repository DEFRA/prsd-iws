namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.PackagingType;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Requests.PackagingType;

    internal class PackagingInfoMap : IMap<SetPackagingInfoForNotification, IEnumerable<PackagingInfo>>,
        IMap<NotificationApplication, PackagingData>
    {
        public IEnumerable<PackagingInfo> Map(SetPackagingInfoForNotification source)
        {
            var packagingInfos = new List<PackagingInfo>();

            foreach (var selectedPackagingType in source.PackagingTypes)
            {
                switch (selectedPackagingType)
                {
                    case PackagingType.Drum:
                    case PackagingType.WoodenBarrel:
                    case PackagingType.Jerrican:
                    case PackagingType.Box:
                    case PackagingType.Bag:
                    case PackagingType.CompositePackaging:
                    case PackagingType.PressureReceptacle:
                    case PackagingType.Bulk:
                        packagingInfos.Add(PackagingInfo.CreatePackagingInfo(selectedPackagingType));
                        break;
                    case PackagingType.Other:
                        packagingInfos.Add(PackagingInfo.CreateOtherPackagingInfo(source.OtherDescription));
                        break;
                    default:
                        throw new InvalidOperationException(string.Format("Unknown unit type {0}", selectedPackagingType));
                }
            }
            return packagingInfos;
        }

        public PackagingData Map(NotificationApplication source)
        {
            var packagingData = new PackagingData();

            foreach (var packagingInfo in source.PackagingInfos)
            {
                packagingData.PackagingTypes.Add(packagingInfo.PackagingType);
            }

            var otherPackaging = source.PackagingInfos.FirstOrDefault(p => p.PackagingType == PackagingType.Other);
            if (otherPackaging != null)
            {
                packagingData.OtherDescription = otherPackaging.OtherDescription;
            }

            return packagingData;
        }
    }
}