namespace EA.Iws.RequestHandlers.WasteCodes
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Notification;
    using Prsd.Core.Mediator;
    using Requests.WasteCodes;

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

            var baselOecdCode = await db.WasteCodes.SingleAsync(w => w.Id == command.BasedOecdCode);
            notification.SetBaselOecdCode(WasteCodeInfo.CreateWasteCodeInfo(baselOecdCode));

            var ewcCodes = new List<WasteCodeInfo>();
            foreach (var ewcId in command.EwcCodes)
            {
                var id = ewcId;
                var wasteCode = await db.WasteCodes.SingleAsync(w => w.Id == id);
                ewcCodes.Add(WasteCodeInfo.CreateWasteCodeInfo(wasteCode));
            }

            notification.SetEwcCodes(ewcCodes);

            await db.SaveChangesAsync();

            return notification.Id;
        }
    }
}