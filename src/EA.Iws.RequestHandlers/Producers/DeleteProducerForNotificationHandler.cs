namespace EA.Iws.RequestHandlers.Producers
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.Producers;

    internal class DeleteProducerForNotificationHandler : IRequestHandler<DeleteProducerForNotification, bool>
    {
        private readonly IProducerRepository repository;
        private readonly IwsContext context;

        public DeleteProducerForNotificationHandler(IwsContext context,
            IProducerRepository repository)
        {
            this.context = context;
            this.repository = repository;
        }

        public async Task<bool> HandleAsync(DeleteProducerForNotification query)
        {
            var producers = await repository.GetByNotificationId(query.NotificationId);

            producers.RemoveProducer(query.ProducerId);

            await context.SaveChangesAsync();

            return true;
        }
    }
}