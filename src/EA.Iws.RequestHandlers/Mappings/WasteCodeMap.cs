namespace EA.Iws.RequestHandlers.Mappings
{
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Requests.WasteType;

    internal class WasteCodeMap : IMap<WasteCode, WasteCodeData>
    {
        public WasteCodeData Map(WasteCode source)
        {
            return new WasteCodeData
            {
                Id = source.Id,
                Description = source.Description,
                Code = source.Code,
                IsOecdCode = source.IsOecdCode
            };
        }
    }
}