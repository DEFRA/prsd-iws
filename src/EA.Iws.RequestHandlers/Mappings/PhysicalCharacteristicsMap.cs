namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.WasteType;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Requests.WasteType;

    internal class PhysicalCharacteristicsMap : IMap<SetPhysicalCharacteristics, IList<PhysicalCharacteristicsInfo>>,
        IMap<NotificationApplication, PhysicalCharacteristicsData>
    {
        public PhysicalCharacteristicsData Map(NotificationApplication source)
        {
            var physicalCharacteristicsData = new PhysicalCharacteristicsData();

            foreach (var physicalCharacteristic in source.PhysicalCharacteristics)
            {
                physicalCharacteristicsData.PhysicalCharacteristics.Add(physicalCharacteristic.PhysicalCharacteristic);
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
                    case PhysicalCharacteristicType.Powdery:
                    case PhysicalCharacteristicType.Solid:
                    case PhysicalCharacteristicType.Viscous:
                    case PhysicalCharacteristicType.Sludgy:
                    case PhysicalCharacteristicType.Liquid:
                    case PhysicalCharacteristicType.Gaseous:
                        physicalCharacteristics.Add(
                            PhysicalCharacteristicsInfo.CreatePhysicalCharacteristicsInfo(
                                physicalCharacteristic));
                        break;
                    case PhysicalCharacteristicType.Other:
                        physicalCharacteristics.Add(
                            PhysicalCharacteristicsInfo.CreateOtherPhysicalCharacteristicsInfo(source.OtherDescription));
                        break;
                    default:
                        throw new InvalidOperationException("Unknown physical characteristic type");
                }
            }

            return physicalCharacteristics;
        }
    }
}