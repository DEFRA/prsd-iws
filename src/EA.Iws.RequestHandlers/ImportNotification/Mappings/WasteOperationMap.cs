namespace EA.Iws.RequestHandlers.ImportNotification.Mappings
{
    using System.Linq;
    using Core.ImportNotification;
    using Core.ImportNotification.Update;
    using Prsd.Core.Mapper;
    using Core = Core.ImportNotification.Summary;
    using Domain = Domain.ImportNotification;

    internal class WasteOperationMap : IMap<Domain.WasteOperation, Core.WasteOperation>,
        IMapWithParameter<Domain.WasteOperation, NotificationDetails, WasteOperationData>
    {
        public Core.WasteOperation Map(Domain.WasteOperation source)
        {
            Core.WasteOperation result = null;

            if (source != null)
            {
                result = new Core.WasteOperation
                {
                    TechnologyEmployed = source.TechnologyEmployed,
                    OperationCodes = source.Codes.Select(x => x.OperationCode).ToList()
                };
            }

            return result;
        }

        public WasteOperationData Map(Domain.WasteOperation source, NotificationDetails parameter)
        {
            var result = new WasteOperationData(parameter.ImportNotificationId) { Details = parameter };

            if (source != null)
            {
                result.OperationCodes = source.Codes.Select(x => x.OperationCode).ToList();
                result.TechnologyEmployed = source.TechnologyEmployed;
            }

            return result;
        }
    }
}