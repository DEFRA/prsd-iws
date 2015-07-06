namespace EA.Iws.RequestHandlers.WasteType
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.WasteType;
    using DataAccess;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.WasteType;
    public class UpdateWasteTypeHandler : IRequestHandler<UpdateWasteType, Guid>
    {
        private readonly IwsContext db;
        private readonly IMap<IList<WoodInformationData>, IList<WasteAdditionalInformation>> wasteTypeMap;
        public UpdateWasteTypeHandler(IwsContext db, IMap<IList<WoodInformationData>, IList<WasteAdditionalInformation>> wasteTypeMap)
        {
            this.db = db;
            this.wasteTypeMap = wasteTypeMap;
        }
        public async Task<Guid> HandleAsync(UpdateWasteType command)
        {
            var notification = await db.NotificationApplications.SingleAsync(n => n.Id == command.NotificationId);

            notification.SetWasteAdditionalInformation(wasteTypeMap.Map(command.WasteCompositions));

            await db.SaveChangesAsync();
            return notification.WasteType.Id;
        }
    }
}