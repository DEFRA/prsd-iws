namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.WasteType;
    using Prsd.Core.Domain;
    using Prsd.Core.Mapper;
    using Requests.WasteType;
    internal class CodeTypeMap : IMap<CodeType, Domain.Notification.CodeType>
    {
        public Domain.Notification.CodeType Map(CodeType source)
        {
            return Enumeration.FromValue<Domain.Notification.CodeType>((int)source);
        }
    }
}