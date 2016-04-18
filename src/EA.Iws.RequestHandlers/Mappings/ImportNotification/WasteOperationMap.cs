namespace EA.Iws.RequestHandlers.Mappings.ImportNotification
{
    using System.Linq;
    using Core.OperationCodes;
    using Domain.ImportNotification;
    using Prsd.Core.Mapper;
    using ImportNotification = Domain.ImportNotification.ImportNotification;
    using WasteOperation = Core.ImportNotification.Draft.WasteOperation;

    internal class WasteOperationMap :
        IMapWithParameter<WasteOperation, ImportNotification, Domain.ImportNotification.WasteOperation>
    {
        public Domain.ImportNotification.WasteOperation Map(WasteOperation source, ImportNotification parameter)
        {
            var wasteOperation = new Domain.ImportNotification.WasteOperation(source.ImportNotificationId,
                OperationCodesList.CreateForNotification(parameter,
                    source.OperationCodes.Select(o => (OperationCode)o)));

            if (!string.IsNullOrWhiteSpace(source.TechnologyEmployed))
            {
                wasteOperation.SetTechnologyEmployed(source.TechnologyEmployed);
            }

            return wasteOperation;
        }
    }
}