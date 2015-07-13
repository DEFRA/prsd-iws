namespace EA.Iws.RequestHandlers.Producers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Producers;

    internal class DeleteProducerForNotificationHandler : IRequestHandler<DeleteProducerForNotification, bool>
    {
        private readonly IwsContext context;

        public DeleteProducerForNotificationHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<bool> HandleAsync(DeleteProducerForNotification query)
        {
            var notification = await context.GetNotificationApplication(query.NotificationId);
            notification.RemoveProducer(query.ProducerId);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
