namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using System.Collections.Generic;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Requests.WasteType;

    public class WasteTypeMap : IMap<WasteType, WasteTypeData>, IMap<CreateWasteType, WasteType>
    {
        private readonly IMap<IList<WasteCompositionData>, IList<WasteComposition>> wasteCompositionMapper;

        public WasteTypeMap(IMap<IList<WasteCompositionData>, IList<WasteComposition>> wasteCompositionMapper)
        {
            this.wasteCompositionMapper = wasteCompositionMapper;
        }

        public WasteTypeData Map(WasteType source)
        {
            return new WasteTypeData
            {
                Id = source.Id,
                ChemicalCompositionName = source.ChemicalCompositionName,
                ChemicalCompositionDescription = source.ChemicalCompositionDescription
            };
        }

        public WasteType Map(CreateWasteType source)
        {
            WasteType wasteType;
            switch (source.ChemicalCompositionType)
            {
                case ChemicalCompositionType.RDF:
                    wasteType = WasteType.CreateRdfWasteType(wasteCompositionMapper.Map(source.WasteCompositions));
                    break;
                case ChemicalCompositionType.SRF:
                    wasteType = WasteType.CreateSrfWasteType(wasteCompositionMapper.Map(source.WasteCompositions));
                    break;
                case ChemicalCompositionType.Wood:
                    wasteType = WasteType.CreateWoodWasteType(source.ChemicalCompositionDescription);
                    break;
                case ChemicalCompositionType.Other:
                    wasteType = WasteType.CreateOtherWasteType(source.ChemicalCompositionName, source.ChemicalCompositionDescription);
                    break;
                default:
                    throw new InvalidOperationException(string.Format("Unknown Chemical Composition Type: {0}", source));
            }
            return wasteType;
        }
    }
}