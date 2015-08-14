namespace EA.Iws.RequestHandlers.WasteCodes
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.WasteCodes;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.WasteCodes;

    internal class SetOtherWasteCodesHandler : IRequestHandler<SetOtherWasteCodes, Guid>
    {
        private readonly IwsContext context;

        public SetOtherWasteCodesHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(SetOtherWasteCodes command)
        {
            var notification = await context.GetNotificationApplication(command.NotificationId);
            
            var exportCode = await context.WasteCodes.SingleAsync(w => w.CodeType == CodeType.ExportCode);
            var importCode = await context.WasteCodes.SingleAsync(w => w.CodeType == CodeType.ImportCode);
            var otherCode = await context.WasteCodes.SingleAsync(w => w.CodeType == CodeType.OtherCode);
            var customsCode = await context.WasteCodes.SingleAsync(w => w.CodeType == CodeType.CustomsCode);
            
            notification.SetExportCode(WasteCodeInfo.CreateCustomWasteCodeInfo(exportCode, command.ExportNationalCode));
            notification.SetImportCode(WasteCodeInfo.CreateCustomWasteCodeInfo(importCode, command.ImportNationalCode));
            notification.SetCustomsCode(WasteCodeInfo.CreateCustomWasteCodeInfo(customsCode, command.CustomsCode));
            notification.SetOtherCode(WasteCodeInfo.CreateCustomWasteCodeInfo(otherCode, command.OtherCode));

            await context.SaveChangesAsync();
            return notification.Id;
        }
    }
}