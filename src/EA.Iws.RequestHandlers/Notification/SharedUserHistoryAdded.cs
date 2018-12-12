namespace EA.Iws.RequestHandlers.Notification
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core;
    using Prsd.Core.Domain;

    internal class SharedUserHistoryAdded : IEventHandler<NotificationSharedUserHistoryAddedEvent>
    {
        private readonly ISharedUserHistoryRepository repository;
        private readonly IwsContext context;

        public SharedUserHistoryAdded(ISharedUserHistoryRepository repository, IwsContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        public async Task HandleAsync(NotificationSharedUserHistoryAddedEvent @event)
        {
            var sharedUser = new SharedUserHistory(
                @event.NotificationId,
                @event.UserId,
                @event.DateAdded,
                SystemTime.UtcNow);

            repository.AddSharedUserHistory(sharedUser);

            await context.SaveChangesAsync();
        }
    }
}
