namespace EA.Iws.RequestHandlers.Notification
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core;
    using Prsd.Core.Domain;

    internal class SharedUserAdded : IEventHandler<NotificationSharedUserAddedEvent>
    {
        private readonly ISharedUserRepository repository;
        private readonly IwsContext context;

        public SharedUserAdded(ISharedUserRepository repository, IwsContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        public async Task HandleAsync(NotificationSharedUserAddedEvent @event)
        {
            var sharedUser = new SharedUser(
                @event.NotificationId,
                @event.UserId,
                SystemTime.UtcNow);

            await repository.AddSharedUser(sharedUser);

            await context.SaveChangesAsync();
        }
    }
}
