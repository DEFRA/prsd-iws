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

    internal class SetBaselOrOecdWasteCodeHandler : IRequestHandler<SetBaselOrOecdWasteCode, Guid>
    {
        private readonly IwsContext db;

        public SetBaselOrOecdWasteCodeHandler(IwsContext db)
        {
            this.db = db;
        }

        public async Task<Guid> HandleAsync(SetBaselOrOecdWasteCode command)
        {
            var notification = await db.NotificationApplications.Include(n => n.ShipmentInfo).SingleAsync(n => n.Id == command.NotificationId);
            var wasteCode = await db.WasteCodes.SingleAsync(w => w.Id == command.WasteCodeId);

            notification.AddWasteCode(wasteCode);

            await db.SaveChangesAsync();

            return wasteCode.Id;
        }
    }
}