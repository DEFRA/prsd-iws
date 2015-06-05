namespace EA.Iws.RequestHandlers.Mappings
{
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Requests.WasteType;

    public class WasteTypeMap : IMap<WasteType, WasteTypeData>
    {
        public WasteTypeData Map(WasteType source)
        {
            return new WasteTypeData
            {
                Id = source.Id,
                ChemicalCompositionName = source.ChemicalCompositionName,
                ChemicalCompositionDescription = source.ChemicalCompositionDescription
            };
        }
    }
}
