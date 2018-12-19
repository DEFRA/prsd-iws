namespace EA.Iws.RequestHandlers.Notification
{
    using DataAccess;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using System.Data.Entity;
    using System.Threading.Tasks;

    internal class CheckIfNotificationOwnerHandler : IRequestHandler<CheckIfNotificationOwner, bool>
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;

        public CheckIfNotificationOwnerHandler(IwsContext context, IUserContext userContext)
        {
            this.context = context;
            this.userContext = userContext;
        }

        public async Task<bool> HandleAsync(CheckIfNotificationOwner message)
        {
            var notificationOwner = (await context.NotificationApplications.SingleAsync(n => n.Id == message.NotificationId)).UserId;

            if (notificationOwner.CompareTo(userContext.UserId) == 0)
            {
                return true;
            }
            return false;
        }
    }
}
