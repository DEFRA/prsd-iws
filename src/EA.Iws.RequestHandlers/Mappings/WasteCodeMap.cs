namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Requests.WasteType;
    using CodeType = Requests.WasteType.CodeType;

    internal class WasteCodeMap : IMap<WasteCode, WasteCodeData>
    {
        public WasteCodeData Map(WasteCode source)
        {
            return new WasteCodeData
            {
                Id = source.Id,
                Description = source.Description,
                Code = source.Code,
                CodeType = GetCodeType(source.CodeType)
            };
        }

        private CodeType GetCodeType(Domain.Notification.CodeType codeType)
        {
            CodeType type;
            if (Enum.TryParse(codeType.Value.ToString(), out type))
            {
                return type;
            }
            throw new ArgumentException(string.Format("Unknown CodeType {0}", codeType.Value), "codeType");
        }
    }
}