namespace EA.Iws.RequestHandlers.WasteType
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Notification;
    using Prsd.Core.Mediator;
    using Requests.WasteType;

    internal class SetWasteCodesHandler : IRequestHandler<SetWasteCodes, Guid>
    {
        private readonly IwsContext db;

        public SetWasteCodesHandler(IwsContext db)
        {
            this.db = db;
        }

        public async Task<Guid> HandleAsync(SetWasteCodes command)
        {
            var notification = await db.NotificationApplications.SingleAsync(n => n.Id == command.NotificationId);

            foreach (var code in command.WasteCodes)
            {
                var id = code.Id;
                var wasteCode = await db.WasteCodes.SingleAsync(w => w.Id == id);
                notification.AddWasteCode(WasteCodeInfo.CreateWasteCodeInfo(wasteCode));
            }

            await db.SaveChangesAsync();

            return notification.Id;
        }
    }
}