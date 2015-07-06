namespace EA.Iws.RequestHandlers.WasteType
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.WasteType;

    public class CreateWasteTypeHandler : IRequestHandler<CreateWasteType, Guid>
    {
        private readonly IwsContext db;
        private readonly IMap<CreateWasteType, WasteType> wasteTypeMap;

        public CreateWasteTypeHandler(IwsContext db, IMap<CreateWasteType, WasteType> wasteTypeMap)
        {
            this.db = db;
            this.wasteTypeMap = wasteTypeMap;
        }

        public async Task<Guid> HandleAsync(CreateWasteType command)
        {
            var notification = await db.NotificationApplications.SingleAsync(n => n.Id == command.NotificationId);

            notification.SetWasteType(wasteTypeMap.Map(command));

            await db.SaveChangesAsync();

            return notification.WasteType.Id;
        }
    }
}