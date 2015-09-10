namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.WasteType;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Requests.WasteType;

    public class WasteTypeMap : IMap<WasteType, WasteTypeData>, IMap<CreateWasteType, WasteType>
    {
        private readonly IMap<IList<WasteTypeCompositionData>, IList<WasteComposition>> wasteCompositionMapper;
        private readonly IMap<IEnumerable<WasteComposition>, IList<EA.Iws.Core.WasteType.WasteCompositionData>> wasteTypeDataMapper;
        private readonly IMap<IEnumerable<WasteAdditionalInformation>, IList<EA.Iws.Core.WasteType.WoodInformationData>> wasteAdditionalInformationMapper;

        public WasteTypeMap(IMap<IList<WasteTypeCompositionData>, IList<WasteComposition>> wasteCompositionMapper,
            IMap<IEnumerable<WasteComposition>, IList<EA.Iws.Core.WasteType.WasteCompositionData>> wasteTypeDataMapper,
            IMap<IEnumerable<WasteAdditionalInformation>, IList<EA.Iws.Core.WasteType.WoodInformationData>> wasteAdditionalInformationMapper)
        {
            this.wasteCompositionMapper = wasteCompositionMapper;
            this.wasteTypeDataMapper = wasteTypeDataMapper;
            this.wasteAdditionalInformationMapper = wasteAdditionalInformationMapper;
        }

        public WasteTypeData Map(WasteType source)
        {
            return new WasteTypeData
            {
                Id = source.Id,
                ChemicalCompositionName = source.ChemicalCompositionName,
                ChemicalCompositionType = GetChemicalCompositionType(source.ChemicalCompositionType),
                ChemicalCompositionDescription = source.ChemicalCompositionDescription,
                OtherWasteTypeDescription = source.OtherWasteTypeDescription,
                WasteCompositionData = wasteTypeDataMapper.Map(source.WasteCompositions).ToList(),
                HasAnnex = source.HasAnnex,
                WasteAdditionalInformation = wasteAdditionalInformationMapper.Map(source.WasteAdditionalInformation).ToList(),
                EnergyInformation = source.EnergyInformation,
                FurtherInformation = source.OptionalInformation,
                WoodTypeDescription = source.WoodTypeDescription
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
                    wasteType = WasteType.CreateWoodWasteType(source.ChemicalCompositionDescription, wasteCompositionMapper.Map(source.WasteCompositions));
                    break;
                case ChemicalCompositionType.Other:
                    wasteType = WasteType.CreateOtherWasteType(source.WasteCompositionName);
                    break;
                default:
                    throw new InvalidOperationException(string.Format("Unknown Chemical Composition Type: {0}", source));
            }
            return wasteType;
        }

        private static ChemicalCompositionType GetChemicalCompositionType(ChemicalComposition chemicalComposition)
        {
            ChemicalCompositionType type;
            if (Enum.TryParse(chemicalComposition.Value.ToString(), out type))
            {
                return type;
            }
            throw new ArgumentException(string.Format("Unknown Chemical Composition {0}", chemicalComposition.Value), "chemicalCompositionType");
        }
    }
}