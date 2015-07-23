namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using System.Collections.Generic;
    using Core.WasteType;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;

    internal class WasteAdditionalInformationMap : IMap<IList<WoodInformationData>, IList<WasteAdditionalInformation>>
    {
        public IList<WasteAdditionalInformation> Map(IList<WoodInformationData> source)
        {
            var wasteCompositions = new List<WasteAdditionalInformation>();
            foreach (var item in source)
            {
                wasteCompositions.Add(WasteAdditionalInformation.CreateWasteAdditionalInformation(item.Constituent,
                Convert.ToDecimal(item.MinConcentration), Convert.ToDecimal(item.MaxConcentration),
                item.WasteInformationType));
            }
            return wasteCompositions;
        }
    }
}