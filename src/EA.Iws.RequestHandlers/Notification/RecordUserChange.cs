namespace EA.Iws.RequestHandlers.Notification
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core;
    using Prsd.Core.Domain;

    internal class RecordUserChange : IEventHandler<NotificationUserChangedEvent>
    {
        private readonly IUserHistoryRepository repository;
        private readonly IwsContext context;

        public RecordUserChange(IUserHistoryRepository repository, IwsContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        public async Task HandleAsync(NotificationUserChangedEvent @event)
        {
            var userHistory = new UserHistory(
                @event.NotificationId,
                @event.CurrentUserId,
                @event.NewUserId,
                SystemTime.UtcNow);

            repository.AddUserChangeData(userHistory);

            await context.SaveChangesAsync();
        }
    }
}
