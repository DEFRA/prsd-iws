namespace EA.Iws.RequestHandlers.Notification
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class ChangeUserHandler : IRequestHandler<ChangeUser, bool>
    {
        private readonly IwsContext context;
        private readonly ChangeNotificationUser changeNotificationUser;

        public ChangeUserHandler(ChangeNotificationUser changeNotificationUser, IwsContext context)
        {
            this.changeNotificationUser = changeNotificationUser;
            this.context = context;
        }

        public async Task<bool> HandleAsync(ChangeUser message)
        {
            await changeNotificationUser.Apply(message.NotificationId, message.NewUserId);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
