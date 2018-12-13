namespace EA.Iws.RequestHandlers.Notification
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class AddSharedUserHandler : IRequestHandler<AddSharedUser, bool>
    {
        private readonly IwsContext context;
        private readonly AddNotificationSharedUser addNotificationSharedUser;

        public AddSharedUserHandler(AddNotificationSharedUser addNotificationSharedUser, IwsContext context)
        {
            this.addNotificationSharedUser = addNotificationSharedUser;
            this.context = context;
        }

        public async Task<bool> HandleAsync(AddSharedUser message)
        {
            await addNotificationSharedUser.Apply(message.NotificationId, message.UserId);

            await context.SaveChangesAsync();

            return true;
        }
    }
}