namespace EA.Iws.RequestHandlers.WasteType
{
    using DataAccess;
    using Domain.Notification;
    using Prsd.Core.Mediator;
    using Requests.WasteType;
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;

    internal class SetBaselOrOecdWasteCodeHandler : IRequestHandler<SetBaselOrOecdWasteCode, Guid>
    {
        private readonly IwsContext db;

        public SetBaselOrOecdWasteCodeHandler(IwsContext db)
        {
            this.db = db;
        }

        public async Task<Guid> HandleAsync(SetBaselOrOecdWasteCode command)
        {
            var notification = await db.NotificationApplications.SingleAsync(n => n.Id == command.NotificationId);
            var wasteCode = await db.WasteCodes.SingleAsync(w => w.Id == command.WasteCodeId);

            notification.AddWasteCode(WasteCodeInfo.CreateWasteCodeInfo(wasteCode));

            await db.SaveChangesAsync();

            return wasteCode.Id;
        }
    }
}