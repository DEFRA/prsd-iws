namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.WasteType;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Requests.WasteType;
    using PhysicalCharacteristicType = Domain.Notification.PhysicalCharacteristicType;

    internal class PhysicalCharacteristicsMap : IMap<SetPhysicalCharacteristics, IList<PhysicalCharacteristicsInfo>>,
        IMap<NotificationApplication, PhysicalCharacteristicsData>
    {
        public PhysicalCharacteristicsData Map(NotificationApplication source)
        {
            var physicalCharacteristicsData = new PhysicalCharacteristicsData();

            foreach (var physicalCharacteristic in source.PhysicalCharacteristics)
            {
                physicalCharacteristicsData.PhysicalCharacteristics.Add(
                    GetPhysicalCharacteristicType(physicalCharacteristic.PhysicalCharacteristic));
            }

            var otherPhysicalCharacteristic =
                source.PhysicalCharacteristics.FirstOrDefault(
                    p => p.PhysicalCharacteristic == PhysicalCharacteristicType.Other);
            if (otherPhysicalCharacteristic != null)
            {
                physicalCharacteristicsData.OtherDescription = otherPhysicalCharacteristic.OtherDescription;
            }

            return physicalCharacteristicsData;
        }

        public IList<PhysicalCharacteristicsInfo> Map(SetPhysicalCharacteristics source)
        {
            var physicalCharacteristics = new List<PhysicalCharacteristicsInfo>();

            foreach (var physicalCharacteristic in source.PhysicalCharacteristics)
            {
                switch (physicalCharacteristic)
                {
                    case Core.WasteType.PhysicalCharacteristicType.Powdery:
                        physicalCharacteristics.Add(
                            PhysicalCharacteristicsInfo.CreatePhysicalCharacteristicsInfo(
                                PhysicalCharacteristicType.Powdery));
                        break;
                    case Core.WasteType.PhysicalCharacteristicType.Solid:
                        physicalCharacteristics.Add(
                            PhysicalCharacteristicsInfo.CreatePhysicalCharacteristicsInfo(
                                PhysicalCharacteristicType.Solid));
                        break;
                    case Core.WasteType.PhysicalCharacteristicType.Viscous:
                        physicalCharacteristics.Add(
                            PhysicalCharacteristicsInfo.CreatePhysicalCharacteristicsInfo(
                                PhysicalCharacteristicType.Viscous));
                        break;
                    case Core.WasteType.PhysicalCharacteristicType.Sludgy:
                        physicalCharacteristics.Add(
                            PhysicalCharacteristicsInfo.CreatePhysicalCharacteristicsInfo(
                                PhysicalCharacteristicType.Sludgy));
                        break;
                    case Core.WasteType.PhysicalCharacteristicType.Liquid:
                        physicalCharacteristics.Add(
                            PhysicalCharacteristicsInfo.CreatePhysicalCharacteristicsInfo(
                                PhysicalCharacteristicType.Liquid));
                        break;
                    case Core.WasteType.PhysicalCharacteristicType.Gaseous:
                        physicalCharacteristics.Add(
                            PhysicalCharacteristicsInfo.CreatePhysicalCharacteristicsInfo(
                                PhysicalCharacteristicType.Gaseous));
                        break;
                    case Core.WasteType.PhysicalCharacteristicType.Other:
                        physicalCharacteristics.Add(
                            PhysicalCharacteristicsInfo.CreateOtherPhysicalCharacteristicsInfo(source.OtherDescription));
                        break;
                    default:
                        throw new InvalidOperationException("Unknown physical characteristic type");
                }
            }

            return physicalCharacteristics;
        }

        public IList<PhysicalCharacteristicType> Map(IList<Core.WasteType.PhysicalCharacteristicType> codes)
        {
            var physicalCharacteristics = new List<PhysicalCharacteristicType>();

            foreach (var selectedphysicalCharacteristic in codes)
            {
                switch (selectedphysicalCharacteristic)
                {
                    case Core.WasteType.PhysicalCharacteristicType.Powdery:
                        physicalCharacteristics.Add(PhysicalCharacteristicType.Powdery);
                        break;
                    case Core.WasteType.PhysicalCharacteristicType.Solid:
                        physicalCharacteristics.Add(PhysicalCharacteristicType.Solid);
                        break;
                    case Core.WasteType.PhysicalCharacteristicType.Viscous:
                        physicalCharacteristics.Add(PhysicalCharacteristicType.Viscous);
                        break;
                    case Core.WasteType.PhysicalCharacteristicType.Sludgy:
                        physicalCharacteristics.Add(PhysicalCharacteristicType.Sludgy);
                        break;
                    case Core.WasteType.PhysicalCharacteristicType.Liquid:
                        physicalCharacteristics.Add(PhysicalCharacteristicType.Liquid);
                        break;
                    case Core.WasteType.PhysicalCharacteristicType.Gaseous:
                        physicalCharacteristics.Add(PhysicalCharacteristicType.Gaseous);
                        break;
                    case Core.WasteType.PhysicalCharacteristicType.Other:
                        physicalCharacteristics.Add(PhysicalCharacteristicType.Other);
                        break;
                    default:
                        throw new InvalidOperationException("Unknown physical characteristic type");
                }
            }
            return physicalCharacteristics;
        }

        private static Core.WasteType.PhysicalCharacteristicType GetPhysicalCharacteristicType(
            PhysicalCharacteristicType physicalCharacteristicType)
        {
            Core.WasteType.PhysicalCharacteristicType type;
            if (Enum.TryParse(physicalCharacteristicType.Value.ToString(), out type))
            {
                return type;
            }
            throw new ArgumentException(string.Format("Unknown PackagingType {0}", physicalCharacteristicType.Value),
                "physicalCharacteristicType");
        }
    }
}