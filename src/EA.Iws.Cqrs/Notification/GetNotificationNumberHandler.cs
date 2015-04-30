namespace EA.Iws.Cqrs.Notification
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.Cqrs;
    using DataAccess;

    public class GetNotificationNumberHandler : IQueryHandler<GetNotificationNumber, string>
    {
        private readonly IwsContext context;

        public GetNotificationNumberHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<string> ExecuteAsync(GetNotificationNumber query)
        {
            // TODO - check if user has access to this notification
            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == query.NotificationId);
            return notification.NotificationNumber;
        }
    }
}