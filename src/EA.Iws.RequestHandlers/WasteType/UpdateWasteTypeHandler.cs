namespace EA.Iws.RequestHandlers.WasteType
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.WasteType;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.WasteType;

    public class UpdateWasteTypeHandler : IRequestHandler<UpdateWasteType, Guid>
    {
        private readonly IwsContext context;
        private readonly IMap<IList<WasteTypeCompositionData>, IList<WasteComposition>> wasteTypeMap;

        public UpdateWasteTypeHandler(IwsContext context,
            IMap<IList<WasteTypeCompositionData>, IList<WasteComposition>> wasteTypeMap)
        {
            this.context = context;
            this.wasteTypeMap = wasteTypeMap;
        }

        public async Task<Guid> HandleAsync(UpdateWasteType command)
        {
            var notification = await context.GetNotificationApplication(command.NotificationId);

            notification.SetChemicalCompostitionContinues(wasteTypeMap.Map(command.WasteCompositions));

            await context.SaveChangesAsync();
            return notification.WasteType.Id;
        }
    }
}