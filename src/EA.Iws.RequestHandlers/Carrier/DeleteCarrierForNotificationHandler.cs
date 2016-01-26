namespace EA.Iws.RequestHandlers.Carrier
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.Carriers;

    internal class DeleteCarrierForNotificationHandler : IRequestHandler<DeleteCarrierForNotification, bool>
    {
        private readonly ICarrierRepository repository;
        private readonly IwsContext context;

        public DeleteCarrierForNotificationHandler(IwsContext context,
            ICarrierRepository repository)
        {
            this.context = context;
            this.repository = repository;
        }

        public async Task<bool> HandleAsync(DeleteCarrierForNotification query)
        {
            var carriers = await repository.GetByNotificationId(query.NotificationId);

            carriers.RemoveCarrier(query.CarrierId);

            await context.SaveChangesAsync();

            return true;
        }
    }
}