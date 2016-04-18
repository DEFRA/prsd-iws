namespace EA.Iws.RequestHandlers.Notification
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Annexes;
    using Prsd.Core.Domain;

    public class CreateAnnexCollection : IEventHandler<NotificationCreatedEvent>
    {
        private readonly IwsContext context;
        private readonly IAnnexCollectionRepository annexCollectionRepository;

        public CreateAnnexCollection(IwsContext context, IAnnexCollectionRepository annexCollectionRepository)
        {
            this.context = context;
            this.annexCollectionRepository = annexCollectionRepository;
        }

        public async Task HandleAsync(NotificationCreatedEvent @event)
        {
            var annexCollection = new AnnexCollection(@event.Notification.Id);

            annexCollectionRepository.Add(annexCollection);

            await context.SaveChangesAsync();
        }
    }
}
