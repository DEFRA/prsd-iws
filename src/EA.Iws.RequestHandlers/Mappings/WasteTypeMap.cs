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
        private readonly IMap<IList<WoodInformationData>, IList<WasteAdditionalInformation>> compositionContinuedMapper;
        private readonly IMap<IEnumerable<WasteComposition>, IList<WasteCompositionData>> wasteTypeDataMapper;
        private readonly IMap<IEnumerable<WasteAdditionalInformation>, IList<WoodInformationData>> compositionMapper;

        public WasteTypeMap(IMap<IList<WoodInformationData>, IList<WasteAdditionalInformation>> compositionContinuedMapper,
            IMap<IEnumerable<WasteComposition>, IList<WasteCompositionData>> wasteTypeDataMapper,
            IMap<IEnumerable<WasteAdditionalInformation>, IList<WoodInformationData>> compositionMapper)
        {
            this.compositionContinuedMapper = compositionContinuedMapper;
            this.wasteTypeDataMapper = wasteTypeDataMapper;
            this.compositionMapper = compositionMapper;
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
                WasteAdditionalInformation = compositionMapper.Map(source.WasteAdditionalInformation).ToList(),
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
                case ChemicalComposition.RDF:
                    wasteType = WasteType.CreateRdfWasteType(compositionContinuedMapper.Map(source.WasteCompositions));
                    break;
                case ChemicalComposition.SRF:
                    wasteType = WasteType.CreateSrfWasteType(compositionContinuedMapper.Map(source.WasteCompositions));
                    break;
                case ChemicalComposition.Wood:
                    wasteType = WasteType.CreateWoodWasteType(source.ChemicalCompositionDescription, compositionContinuedMapper.Map(source.WasteCompositions));
                    break;
                case ChemicalComposition.Other:
                    wasteType = WasteType.CreateOtherWasteType(source.WasteCompositionName);
                    break;
                default:
                    throw new InvalidOperationException(string.Format("Unknown Chemical Composition Type: {0}", source));
            }
            return wasteType;
        }

        private static ChemicalComposition GetChemicalCompositionType(ChemicalComposition chemicalComposition)
        {
            ChemicalComposition type;

            if (Enum.TryParse(chemicalComposition.ToString(), out type))
            {
                return type;
            }

            throw new ArgumentException(string.Format("Unknown Chemical Composition {0}", chemicalComposition), "chemicalComposition");
        }
    }
}