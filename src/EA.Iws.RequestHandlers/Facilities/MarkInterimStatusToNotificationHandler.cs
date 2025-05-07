namespace EA.Iws.RequestHandlers.Facilities
{
    using EA.Iws.DataAccess;
    using EA.Iws.Domain.NotificationAssessment;
    using EA.Iws.Requests.Facilities;
    using EA.Prsd.Core.Mediator;
    using System.Threading.Tasks;

    internal class MarkInterimStatusToNotificationHandler : IRequestHandler<MarkInterimStatusToNotification, bool>
    {
        private readonly NotificationInterim notificationInterim;
        private readonly IwsContext context;

        public MarkInterimStatusToNotificationHandler(NotificationInterim notificationInterim, IwsContext context)
        {
            this.notificationInterim = notificationInterim;
            this.context = context;
        }

        public async Task<bool> HandleAsync(MarkInterimStatusToNotification message)
        {
            await notificationInterim.SetValue(message.NotificationId, message.IsInterim);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
