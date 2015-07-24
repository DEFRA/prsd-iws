namespace EA.Iws.RequestHandlers.WasteCodes
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.WasteCodes;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.WasteCodes;

    internal class SetUnNumberWasteCodesHandler : IRequestHandler<SetUnNumberWasteCodes, Guid>
    {
        private readonly IwsContext context;

        public SetUnNumberWasteCodesHandler(IwsContext context)
        {
            this.context = context;
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation",
            Justification = "Variable relates to UN numbers")]
        public async Task<Guid> HandleAsync(SetUnNumberWasteCodes message)
        {
            var notification = await context.GetNotificationApplication(message.NotificationId);

            var customsCode = await context.WasteCodes.SingleAsync(w => w.CodeType == CodeType.CustomsCode);

            var unNumbers = new List<WasteCodeInfo>();
            foreach (var unId in message.UnNumbers)
            {
                var id = unId;
                var wasteCode = await context.WasteCodes.SingleAsync(w => w.Id == id);
                unNumbers.Add(WasteCodeInfo.CreateWasteCodeInfo(wasteCode));
            }

            var customsCodes =
                message.CustomsCodes.Select(code => WasteCodeInfo.CreateCustomWasteCodeInfo(customsCode, code)).ToList();

            notification.SetUnNumbers(unNumbers);
            notification.SetCustomsCodes(customsCodes);

            await context.SaveChangesAsync();

            return notification.Id;
        }
    }
}