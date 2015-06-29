namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using System.Collections.Generic;
    using Core.WasteType;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Requests.WasteType;

    internal class WasteCompositionMap : IMap<IList<WasteTypeCompositionData>, IList<WasteComposition>>
    {
        public IList<WasteComposition> Map(IList<WasteTypeCompositionData> source)
        {
            var wasteCompositions = new List<WasteComposition>();
            foreach (var item in source)
            {
                wasteCompositions.Add(WasteComposition.CreateWasteComposition(item.Constituent, Convert.ToDecimal(item.MinConcentration),
                    Convert.ToDecimal(item.MaxConcentration), item.ChemicalCompositionCategory));
            }
            return wasteCompositions;
        }
    }
}