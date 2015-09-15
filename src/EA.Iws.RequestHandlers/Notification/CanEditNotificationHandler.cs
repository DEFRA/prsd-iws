namespace EA.Iws.RequestHandlers.Notification
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class CanEditNotificationHandler : IRequestHandler<CanEditNotification, bool>
    {
        private readonly IwsContext context;

        public CanEditNotificationHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<bool> HandleAsync(CanEditNotification message)
        {
            var assessment =
                await
                    context.NotificationAssessments.Where(p => p.NotificationApplicationId == message.NotificationId)
                        .SingleAsync();

            return assessment.CanEditNotification;
        }
    }
}