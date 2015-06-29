namespace EA.Iws.RequestHandlers.WasteCodes
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Notification;
    using Prsd.Core.Mediator;
    using Requests.WasteCodes;

    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation",
            Justification = "Variables relate to Y/H/UN-Codes")]
    internal class SetYHUnWasteCodesHandler : IRequestHandler<SetYHUnWasteCodes, Guid>
    {
        private readonly IwsContext context;

        public SetYHUnWasteCodesHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(SetYHUnWasteCodes message)
        {
            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == message.NotificationId);

            var yCodes = new List<WasteCodeInfo>();
            foreach (var yCodeId in message.YCodes)
            {
                var id = yCodeId;
                var wasteCode = await context.WasteCodes.SingleAsync(w => w.Id == id);
                yCodes.Add(WasteCodeInfo.CreateWasteCodeInfo(wasteCode));
            }

            var hCodes = new List<WasteCodeInfo>();
            foreach (var hCodeId in message.HCodes)
            {
                var id = hCodeId;
                var wasteCode = await context.WasteCodes.SingleAsync(w => w.Id == id);
                hCodes.Add(WasteCodeInfo.CreateWasteCodeInfo(wasteCode));
            }

            var unClasses = new List<WasteCodeInfo>();
            foreach (var unCodeId in message.UnCodes)
            {
                var id = unCodeId;
                var wasteCode = await context.WasteCodes.SingleAsync(w => w.Id == id);
                unClasses.Add(WasteCodeInfo.CreateWasteCodeInfo(wasteCode));
            }

            notification.SetYCodes(yCodes);
            notification.SetHCodes(hCodes);
            notification.SetUnClasses(unClasses);

            await context.SaveChangesAsync();

            return notification.Id;
        }
    }
}