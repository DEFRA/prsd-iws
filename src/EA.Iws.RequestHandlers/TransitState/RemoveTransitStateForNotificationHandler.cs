namespace EA.Iws.RequestHandlers.TransitState
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.TransportRoute;
    using Prsd.Core.Mediator;
    using Requests.TransitState;

    internal class RemoveTransitStateForNotificationHandler : IRequestHandler<RemoveTransitStateForNotification, bool>
    {
        private readonly IwsContext context;
        private readonly ITransportRouteRepository repository;

        public RemoveTransitStateForNotificationHandler(IwsContext context, ITransportRouteRepository repository)
        {
            this.context = context;
            this.repository = repository;
        }

        public async Task<bool> HandleAsync(RemoveTransitStateForNotification message)
        {
            var transportRoute = await repository.GetByNotificationId(message.NotificationId);

            transportRoute.RemoveTransitState(message.TransitStateId);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
