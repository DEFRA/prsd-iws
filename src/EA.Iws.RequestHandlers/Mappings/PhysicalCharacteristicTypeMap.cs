namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using System.Collections.Generic;
    using Prsd.Core.Mapper;
    using Requests.WasteType;

    internal class PhysicalCharacteristicTypeMap : IMap<IList<PhysicalCharacteristicType>, IList<Domain.PhysicalCharacteristicType>>
    {
        public IList<Domain.PhysicalCharacteristicType> Map(IList<PhysicalCharacteristicType> codes)
        {
            var physicalCharacteristics = new List<Domain.PhysicalCharacteristicType>();

            foreach (var selectedphysicalCharacteristic in codes)
            {
                switch (selectedphysicalCharacteristic)
                {
                    case PhysicalCharacteristicType.Powdery:
                        physicalCharacteristics.Add(Domain.PhysicalCharacteristicType.Powdery);
                        break;
                    case PhysicalCharacteristicType.Solid:
                        physicalCharacteristics.Add(Domain.PhysicalCharacteristicType.Solid);
                        break;
                    case PhysicalCharacteristicType.Viscous:
                        physicalCharacteristics.Add(Domain.PhysicalCharacteristicType.Viscous);
                        break;
                    case PhysicalCharacteristicType.Sludgy:
                        physicalCharacteristics.Add(Domain.PhysicalCharacteristicType.Sludgy);
                        break;
                    case PhysicalCharacteristicType.Liquid:
                        physicalCharacteristics.Add(Domain.PhysicalCharacteristicType.Liquid);
                        break;
                    case PhysicalCharacteristicType.Gaseous:
                        physicalCharacteristics.Add(Domain.PhysicalCharacteristicType.Gaseous);
                        break;
                    case PhysicalCharacteristicType.Other:
                        physicalCharacteristics.Add(Domain.PhysicalCharacteristicType.Other);
                        break;
                    default:
                        throw new InvalidOperationException("Unknown physical characteristic type");
                }
            }
            return physicalCharacteristics;
        }
    }
}