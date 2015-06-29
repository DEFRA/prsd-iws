namespace EA.Iws.RequestHandlers.WasteCodes
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.WasteCodes;
    using DataAccess;
    using Domain.Notification;
    using Prsd.Core.Mediator;
    using Requests.WasteCodes;

    internal class SetOtherWasteCodesHandler : IRequestHandler<SetOtherWasteCodes, Guid>
    {
        private readonly IwsContext db;

        public SetOtherWasteCodesHandler(IwsContext db)
        {
            this.db = db;
        }

        public async Task<Guid> HandleAsync(SetOtherWasteCodes command)
        {
            var notification = await db.NotificationApplications.SingleAsync(n => n.Id == command.NotificationId);
            
            var exportCode = await db.WasteCodes.SingleAsync(w => w.CodeType == CodeType.ExportCode);
            var importCode = await db.WasteCodes.SingleAsync(w => w.CodeType == CodeType.ImportCode);
            var otherCode = await db.WasteCodes.SingleAsync(w => w.CodeType == CodeType.OtherCode);
            
            notification.SetExportCode(WasteCodeInfo.CreateCustomWasteCodeInfo(exportCode, command.ExportNationalCode));
            notification.SetImportCode(WasteCodeInfo.CreateCustomWasteCodeInfo(importCode, command.ImportNationalCode));

            if (string.IsNullOrWhiteSpace(command.OtherCode))
            {
                notification.RemoveOtherCode();
            }
            else
            {
                notification.SetOtherCode(WasteCodeInfo.CreateCustomWasteCodeInfo(otherCode, command.OtherCode));
            }

            await db.SaveChangesAsync();
            return notification.Id;
        }
    }
}