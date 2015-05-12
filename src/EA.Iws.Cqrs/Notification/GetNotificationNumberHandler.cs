namespace EA.Iws.Cqrs.Notification
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;

    public class GetNotificationNumberHandler : IRequestHandler<GetNotificationNumber, string>
    {
        private readonly IwsContext context;

        public GetNotificationNumberHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<string> HandleAsync(GetNotificationNumber query)
        {
            // TODO - check if user has access to this notification
            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == query.NotificationId);
            return notification.NotificationNumber;
        }
    }
}