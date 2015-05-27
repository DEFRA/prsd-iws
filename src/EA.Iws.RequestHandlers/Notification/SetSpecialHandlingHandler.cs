namespace EA.Iws.RequestHandlers.Notification
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    public class SetSpecialHandlingHandler : IRequestHandler<SetSpecialHandling, string>
    {
        private readonly IwsContext context;

        public SetSpecialHandlingHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<string> HandleAsync(SetSpecialHandling query)
        {
            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == query.NotificationId);
            notification.SetSpecialHandling(query.IsSpecialHandling);

            await context.SaveChangesAsync();

            return notification.NotificationNumber;
        }
    }
}