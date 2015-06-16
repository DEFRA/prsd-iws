namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Requests.PackagingType;
    using Requests.Shipment;
    using PackagingType = Requests.PackagingType.PackagingType;

    internal class PackagingInfoMap : IMap<SetPackagingTypeOnShipmentInfo, IEnumerable<PackagingInfo>>,
        IMap<NotificationApplication, PackagingData>
    {
        public IEnumerable<PackagingInfo> Map(SetPackagingTypeOnShipmentInfo source)
        {
            var packagingInfos = new List<PackagingInfo>();

            foreach (var selectedPackagingType in source.PackagingTypes)
            {
                switch (selectedPackagingType)
                {
                    case PackagingType.Drum:
                        packagingInfos.Add(PackagingInfo.CreatePackagingInfo(Domain.Notification.PackagingType.Drum));
                        break;
                    case PackagingType.WoodenBarrel:
                        packagingInfos.Add(
                            PackagingInfo.CreatePackagingInfo(Domain.Notification.PackagingType.WoodenBarrel));
                        break;
                    case PackagingType.Jerrican:
                        packagingInfos.Add(PackagingInfo.CreatePackagingInfo(Domain.Notification.PackagingType.Jerrican));
                        break;
                    case PackagingType.Box:
                        packagingInfos.Add(PackagingInfo.CreatePackagingInfo(Domain.Notification.PackagingType.Box));
                        break;
                    case PackagingType.Bag:
                        packagingInfos.Add(PackagingInfo.CreatePackagingInfo(Domain.Notification.PackagingType.Bag));
                        break;
                    case PackagingType.CompositePackaging:
                        packagingInfos.Add(
                            PackagingInfo.CreatePackagingInfo(Domain.Notification.PackagingType.CompositePackaging));
                        break;
                    case PackagingType.PressureReceptacle:
                        packagingInfos.Add(
                            PackagingInfo.CreatePackagingInfo(Domain.Notification.PackagingType.PressureReceptacle));
                        break;
                    case PackagingType.Bulk:
                        packagingInfos.Add(PackagingInfo.CreatePackagingInfo(Domain.Notification.PackagingType.Bulk));
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
                packagingData.PackagingTypes.Add(GetPackagingType(packagingInfo.PackagingType));
            }

            var otherPackaging =
                source.PackagingInfos.FirstOrDefault(p => p.PackagingType == Domain.Notification.PackagingType.Other);
            if (otherPackaging != null)
            {
                packagingData.OtherDescription = otherPackaging.OtherDescription;
            }

            return packagingData;
        }

        private PackagingType GetPackagingType(Domain.Notification.PackagingType packagingType)
        {
            PackagingType type;
            if (Enum.TryParse(packagingType.Value.ToString(), out type))
            {
                return type;
            }
            throw new ArgumentException(string.Format("Unknown PackagingType {0}", packagingType.Value), "packagingType");
        }
    }
}