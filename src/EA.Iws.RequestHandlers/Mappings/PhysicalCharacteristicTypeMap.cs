namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using System.Collections.Generic;
    using Prsd.Core.Mapper;
    using Requests.WasteType;

    internal class PhysicalCharacteristicTypeMap : IMap<IList<PhysicalCharacteristicType>, IList<Domain.Notification.PhysicalCharacteristicType>>
    {
        public IList<Domain.Notification.PhysicalCharacteristicType> Map(IList<PhysicalCharacteristicType> codes)
        {
            var physicalCharacteristics = new List<Domain.Notification.PhysicalCharacteristicType>();

            foreach (var selectedphysicalCharacteristic in codes)
            {
                switch (selectedphysicalCharacteristic)
                {
                    case PhysicalCharacteristicType.Powdery:
                        physicalCharacteristics.Add(Domain.Notification.PhysicalCharacteristicType.Powdery);
                        break;
                    case PhysicalCharacteristicType.Solid:
                        physicalCharacteristics.Add(Domain.Notification.PhysicalCharacteristicType.Solid);
                        break;
                    case PhysicalCharacteristicType.Viscous:
                        physicalCharacteristics.Add(Domain.Notification.PhysicalCharacteristicType.Viscous);
                        break;
                    case PhysicalCharacteristicType.Sludgy:
                        physicalCharacteristics.Add(Domain.Notification.PhysicalCharacteristicType.Sludgy);
                        break;
                    case PhysicalCharacteristicType.Liquid:
                        physicalCharacteristics.Add(Domain.Notification.PhysicalCharacteristicType.Liquid);
                        break;
                    case PhysicalCharacteristicType.Gaseous:
                        physicalCharacteristics.Add(Domain.Notification.PhysicalCharacteristicType.Gaseous);
                        break;
                    case PhysicalCharacteristicType.Other:
                        physicalCharacteristics.Add(Domain.Notification.PhysicalCharacteristicType.Other);
                        break;
                    default:
                        throw new InvalidOperationException("Unknown physical characteristic type");
                }
            }
            return physicalCharacteristics;
        }
    }
}