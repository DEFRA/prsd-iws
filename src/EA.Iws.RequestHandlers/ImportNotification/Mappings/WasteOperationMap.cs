namespace EA.Iws.RequestHandlers.ImportNotification.Mappings
{
    using System.Linq;
    using Prsd.Core.Mapper;
    using Core = Core.ImportNotification.Summary;
    using Domain = Domain.ImportNotification;

    internal class WasteOperationMap : IMap<Domain.WasteOperation, Core.WasteOperation>
    {
        public Core.WasteOperation Map(Domain.WasteOperation source)
        {
            return new Core.WasteOperation
            {
                TechnologyEmployed = source.TechnologyEmployed,
                OperationCodes = source.Codes.Select(x => x.OperationCode.Value).ToArray()
            };
        }
    }
}