namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.WasteType;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Requests.WasteType;

    internal class WasteCompositionMap : IMap<IList<WasteTypeCompositionData>, IList<WasteComposition>>, 
                                         IMap<IEnumerable<WasteComposition>, IList<WasteCompositionData>>,  
                                         IMap<IEnumerable<WasteAdditionalInformation>, IList<WoodInformationData>>,
                                         IMap<IList<WoodInformationData>, IList<WasteAdditionalInformation>>
    {
        public IList<WasteComposition> Map(IList<WasteTypeCompositionData> source)
        {
            return source.OrderBy(x => x.ChemicalCompositionCategory)
                .Select(item => WasteComposition.CreateWasteComposition(
                    item.Constituent, 
                    Convert.ToDecimal(item.MinConcentration), 
                    Convert.ToDecimal(item.MaxConcentration), 
                    item.ChemicalCompositionCategory)).ToList();
        }

        public IList<WasteAdditionalInformation> Map(IList<WoodInformationData> source)
        {
            return source.OrderBy(x => x.WasteInformationType)
                .Select(item => WasteAdditionalInformation.CreateWasteAdditionalInformation(
                    item.Constituent, 
                    Convert.ToDecimal(item.MinConcentration), 
                    Convert.ToDecimal(item.MaxConcentration), 
                    item.WasteInformationType)).ToList();
        }

        public IList<WasteCompositionData> Map(IEnumerable<WasteComposition> source)
        {
            return source.OrderBy(x => x.ChemicalCompositionType)
                .Select(item => new WasteCompositionData
                {
                    ChemicalCompositionCategory = item.ChemicalCompositionType,
                    MinConcentration = item.MinConcentration,
                    MaxConcentration = item.MaxConcentration,
                    Constituent = item.Constituent
                }).ToList();
        }

        public IList<WoodInformationData> Map(IEnumerable<WasteAdditionalInformation> source)
        {
            return source.OrderBy(x => x.WasteInformationType)
                .Select(item => new WoodInformationData
                {
                    Constituent = item.Constituent,
                    WasteInformationType = item.WasteInformationType,
                    MinConcentration = item.MinConcentration.ToString(),
                    MaxConcentration = item.MaxConcentration.ToString()
                }).ToList();
        }
    }
}