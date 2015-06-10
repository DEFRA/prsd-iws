namespace EA.Iws.RequestHandlers.Mappings
{
    using System.Collections.Generic;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Requests.WasteType;

    internal class WasteCompositionMap : IMap<IList<WasteCompositionData>, IList<WasteComposition>>
    {
        public IList<WasteComposition> Map(IList<WasteCompositionData> source)
        {
            var wasteCompositions = new List<WasteComposition>();
            foreach (var item in source)
            {
                wasteCompositions.Add(WasteComposition.CreateWasteComposition(item.Constituent, item.MinConcentration,
                    item.MaxConcentration));
            }
            return wasteCompositions;
        }
    }
}