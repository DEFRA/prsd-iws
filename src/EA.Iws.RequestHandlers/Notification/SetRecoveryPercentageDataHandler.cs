namespace EA.Iws.RequestHandlers.Notification
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    public class SetRecoveryPercentageDataHandler : IRequestHandler<SetRecoveryPercentageData, Guid>
    {
        private readonly IwsContext context;

        public SetRecoveryPercentageDataHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(SetRecoveryPercentageData message)
        {
            var notification = await context.GetNotificationApplication(message.NotificationId);

            if (message.IsProvidedByImporter)
            {
                notification.SetRecoveryPercentageDataProvidedByImporter();
            }
            else
            {
                notification.SetRecoveryPercentageData(message.PercentageRecoverable.GetValueOrDefault(), message.MethodOfDisposal);
            }

            await context.SaveChangesAsync();

            return notification.Id;
        }
    }
}
