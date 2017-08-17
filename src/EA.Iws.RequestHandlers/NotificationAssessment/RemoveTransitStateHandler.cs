namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.TransportRoute;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class RemoveTransitStateHandler : IRequestHandler<RemoveTransitState, Unit>
    {
        private readonly IwsContext context;
        private readonly ITransportRouteRepository repository;

        public RemoveTransitStateHandler(IwsContext context, ITransportRouteRepository repository)
        {
            this.context = context;
            this.repository = repository;
        }

        public async Task<Unit> HandleAsync(RemoveTransitState message)
        {
            var transportRoute = await repository.GetByNotificationId(message.NotificationId);
            transportRoute.RemoveTransitState(message.TransitStateId);
            await context.SaveChangesAsync();
            return Unit.Value;
        }
    }
}