namespace EA.Iws.RequestHandlers.WasteCodes
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.WasteCodes;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.WasteCodes;

    internal class SetBaselOecdCodeForNotificationHandler : IRequestHandler<SetBaselOecdCodeForNotification, bool>
    {
        private readonly IwsContext context;

        public SetBaselOecdCodeForNotificationHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<bool> HandleAsync(SetBaselOecdCodeForNotification message)
        {
            var notification = await context.GetNotificationApplication(message.Id);

            var wasteCode = await context.WasteCodes.SingleAsync(wc => wc.Id == message.Code);

            if (message.NotApplicable)
            {
                notification.SetCodesNotApplicable(message.CodeType);
            }
            else
            {
                notification.SetBaselOecdCode(WasteCodeInfo.CreateWasteCodeInfo(wasteCode));
            }

            if (message.CodeType == CodeType.Basel)
            {
                notification.RemoveCodeOfType(CodeType.Oecd);
            }
            else if (message.CodeType == CodeType.Oecd)
            {
                notification.RemoveCodeOfType(CodeType.Basel);
            }

            await context.SaveChangesAsync();

            return true;
        }
    }
}
