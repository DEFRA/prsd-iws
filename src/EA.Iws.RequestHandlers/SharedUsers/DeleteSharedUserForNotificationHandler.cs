namespace EA.Iws.RequestHandlers.SharedUsers
{
    using DataAccess;
    using EA.Iws.Domain.NotificationApplication;
    using EA.Iws.Requests.SharedUsers;
    using EA.Prsd.Core.Mediator;
    using Prsd.Core;
    using System.Threading.Tasks;
    internal class DeleteSharedUserForNotificationHandler : IRequestHandler<DeleteSharedUserForNotification, bool>
    {
        private readonly ISharedUserRepository repository;
        private readonly ISharedUserHistoryRepository historyRepository;
        private readonly IwsContext context;

        public DeleteSharedUserForNotificationHandler(ISharedUserRepository repository, ISharedUserHistoryRepository historyRepository, IwsContext context)
        {
            this.repository = repository;
            this.historyRepository = historyRepository;
            this.context = context;
        }

        public async Task<bool> HandleAsync(DeleteSharedUserForNotification message)
        {
            var sharedUser = await repository.GetSharedUserById(message.NotificationId, message.SharedId);
                        
            historyRepository.AddSharedUserHistory(new SharedUserHistory(
               message.NotificationId,
               sharedUser.UserId,
               sharedUser.DateAdded,
               SystemTime.UtcNow));
            await context.SaveChangesAsync();

            await repository.RemoveSharedUser(message.NotificationId, message.SharedId);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
