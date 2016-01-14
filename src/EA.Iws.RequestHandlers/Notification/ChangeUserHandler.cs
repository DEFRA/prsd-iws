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
        private readonly INotificationUserService service;

        public ChangeUserHandler(INotificationUserService service, IwsContext context)
        {
            this.service = service;
            this.context = context;
        }

        public async Task<bool> HandleAsync(ChangeUser message)
        {
            await service.ChangeNotificationUser(message.NotificationId, message.NewUserId);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
