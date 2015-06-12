namespace EA.Iws.RequestHandlers.WasteType
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.WasteType;

    internal class SetWasteCodeHandler : IRequestHandler<SetWasteCode, Guid>
    {
        private readonly IwsContext db;
        private readonly IMap<IList<PhysicalCharacteristicType>, IList<Domain.Notification.PhysicalCharacteristicType>> mapper;

        public SetWasteCodeHandler(IwsContext db, IMap<IList<PhysicalCharacteristicType>, IList<Domain.Notification.PhysicalCharacteristicType>> mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<Guid> HandleAsync(SetWasteCode command)
        {
            var notification = await db.NotificationApplications.Include(n => n.ShipmentInfo).SingleAsync(n => n.Id == command.NotificationId);
            var wasteCode = await db.WasteCodes.SingleAsync(w => w.Id == command.WasteCodeId);

            notification.AddWasteCode(wasteCode);

            await db.SaveChangesAsync();

            return wasteCode.Id;
        }
    }
}