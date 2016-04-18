namespace EA.Iws.RequestHandlers.WasteCodes
{
    using System;
    using System.Threading.Tasks;
    using Core.WasteCodes;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.WasteCodes;

    internal class SetCustomWasteCodesHandler : IRequestHandler<SetCustomWasteCodes, Guid>
    {
        private readonly IwsContext context;

        public SetCustomWasteCodesHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(SetCustomWasteCodes command)
        {
            var notification = await context.GetNotificationApplication(command.NotificationId);

            if (command.ExportNationalCodeNotApplicable)
            {
                notification.SetCodesNotApplicable(CodeType.ExportCode);
            }
            else
            {
                notification.SetExportCode(WasteCodeInfo.CreateCustomWasteCodeInfo(CodeType.ExportCode, command.ExportNationalCode));
            }

            if (command.ImportNationalCodeNotApplicable)
            {
                notification.SetCodesNotApplicable(CodeType.ImportCode);
            }
            else
            {
                notification.SetImportCode(WasteCodeInfo.CreateCustomWasteCodeInfo(CodeType.ImportCode, command.ImportNationalCode));
            }

            if (command.CustomsCodeNotApplicable)
            {
                notification.SetCodesNotApplicable(CodeType.CustomsCode);
            }
            else
            {
                notification.SetCustomsCode(WasteCodeInfo.CreateCustomWasteCodeInfo(CodeType.CustomsCode, command.CustomsCode));
            }

            if (command.OtherCodeNotApplicable)
            {
                notification.SetCodesNotApplicable(CodeType.OtherCode);
            }
            else
            {
                notification.SetOtherCode(WasteCodeInfo.CreateCustomWasteCodeInfo(CodeType.OtherCode, command.OtherCode));
            }

            await context.SaveChangesAsync();
            return notification.Id;
        }
    }
}