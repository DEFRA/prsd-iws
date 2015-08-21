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

            if (message.NotApplicable)
            {
                SetNotApplicable(message, notification);
            }
            else
            {
                await SetCode(message, notification);
            }

            await context.SaveChangesAsync();

            return true;
        }

        private void SetNotApplicable(SetBaselOecdCodeForNotification message, NotificationApplication notification)
        {
            notification.SetCodesNotApplicable(message.CodeType);

            ClearOldCodes(message, notification);
        }

        private void ClearOldCodes(SetBaselOecdCodeForNotification message, NotificationApplication notification)
        {
            if (message.CodeType == CodeType.Basel)
            {
                notification.RemoveCodeOfType(CodeType.Oecd);
            }
            else if (message.CodeType == CodeType.Oecd)
            {
                notification.RemoveCodeOfType(CodeType.Basel);
            }
        }

        private async Task SetCode(SetBaselOecdCodeForNotification message, NotificationApplication notification)
        {
            var wasteCode = await context.WasteCodes.SingleAsync(wc => wc.Id == message.Code);

            if (wasteCode.CodeType != message.CodeType)
            {
                var errorMessage =
                    string.Format(
                        "Tried to set an invalid code of type {0} where the set for code type {1} was requested. For notification {2}.",
                        wasteCode.CodeType,
                        message.CodeType,
                        message.Id);

                throw new InvalidOperationException(errorMessage);
            }
            
            notification.SetBaselOecdCode(WasteCodeInfo.CreateWasteCodeInfo(wasteCode));

            ClearOldCodes(message, notification);
        }
    }
}
