namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using System.Collections.Generic;
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
            var wasteCompositions = new List<WasteComposition>();
            foreach (var item in source)
            {
                wasteCompositions.Add(
                    WasteComposition.CreateWasteComposition(
                        item.Constituent, 
                        Convert.ToDecimal(item.MinConcentration),
                        Convert.ToDecimal(item.MaxConcentration), 
                        item.ChemicalCompositionCategory));
            }
            return wasteCompositions;
        }
        
        public IList<WasteAdditionalInformation> Map(IList<WoodInformationData> source)
        {
            var wasteCompositions = new List<WasteAdditionalInformation>();
            foreach (var item in source)
            {
                WasteAdditionalInformation.CreateWasteAdditionalInformation(
                    item.Constituent,
                    Convert.ToDecimal(item.MinConcentration),
                    Convert.ToDecimal(item.MaxConcentration),
                    item.WasteInformationType);
            }

            return wasteCompositions;
        }

        public IList<WasteCompositionData> Map(IEnumerable<WasteComposition> source)
        {
            var wasteCompositions = new List<WasteCompositionData>();
            foreach (var item in source)
            {
                wasteCompositions.Add(new WasteCompositionData
                {
                    ChemicalCompositionCategory = item.ChemicalCompositionType,
                    MinConcentration = item.MinConcentration,
                    MaxConcentration = item.MaxConcentration,
                    Constituent = item.Constituent
                });
            }
            return wasteCompositions;
        }

        public IList<WoodInformationData> Map(IEnumerable<WasteAdditionalInformation> source)
        {
            var wasteCompositions = new List<WoodInformationData>();
            foreach (var item in source)
            {
                wasteCompositions.Add(new WoodInformationData
                {
                    Constituent = item.Constituent,
                    WasteInformationType = item.WasteInformationType,
                    MinConcentration = item.MinConcentration.ToString(),
                    MaxConcentration = item.MaxConcentration.ToString()
                });
            }
            return wasteCompositions;
        }
    }
}