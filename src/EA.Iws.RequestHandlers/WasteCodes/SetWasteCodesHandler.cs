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
        private readonly IwsContext context;

        public SetWasteCodesHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(SetWasteCodes command)
        {
            var notification = await context.GetNotificationApplication(command.NotificationId);

            var baselOecdCode = await context.WasteCodes.SingleAsync(w => w.Id == command.BasedOecdCode);
            notification.SetBaselOecdCode(WasteCodeInfo.CreateWasteCodeInfo(baselOecdCode));

            var ewcCodes = new List<WasteCodeInfo>();
            foreach (var ewcId in command.EwcCodes)
            {
                var id = ewcId;
                var wasteCode = await context.WasteCodes.SingleAsync(w => w.Id == id);
                ewcCodes.Add(WasteCodeInfo.CreateWasteCodeInfo(wasteCode));
            }

            notification.SetEwcCodes(ewcCodes);

            await context.SaveChangesAsync();

            return notification.Id;
        }
    }
}