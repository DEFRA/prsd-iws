namespace EA.Iws.RequestHandlers.Notification
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.Notification;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GetUserNotificationsSummaryHandler :
        IRequestHandler<GetUserNotificationsSummary, UserNotificationsSummary>
    {
        private readonly IwsContext context;

        public GetUserNotificationsSummaryHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<UserNotificationsSummary> HandleAsync(GetUserNotificationsSummary message)
        {
            var numberOfNotifications =
                await context.NotificationApplications.CountAsync(n => n.UserId == message.UserId);
            return new UserNotificationsSummary
            {
                NumberOfNotifications = numberOfNotifications
            };
        }
    }
}