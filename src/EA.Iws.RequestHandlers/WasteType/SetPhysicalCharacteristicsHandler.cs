namespace EA.Iws.RequestHandlers.WasteType
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.WasteType;

    internal class SetPhysicalCharacteristicsHandler : IRequestHandler<SetPhysicalCharacteristics, Guid>
    {
        private readonly IwsContext db;
        private readonly IMap<SetPhysicalCharacteristics, IList<PhysicalCharacteristicsInfo>> mapper;

        public SetPhysicalCharacteristicsHandler(IwsContext db, IMap<SetPhysicalCharacteristics, IList<PhysicalCharacteristicsInfo>> mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<Guid> HandleAsync(SetPhysicalCharacteristics command)
        {
            var notification = await db.NotificationApplications.SingleAsync(n => n.Id == command.NotificationId);
            var physicalCharacteristics = mapper.Map(command);

            notification.SetPhysicalCharacteristics(physicalCharacteristics);

            await db.SaveChangesAsync();

            return notification.Id;
        }
    }
}