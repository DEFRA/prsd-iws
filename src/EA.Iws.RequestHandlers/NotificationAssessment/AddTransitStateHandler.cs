namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.TransportRoute;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class AddTransitStateHandler : IRequestHandler<AddTransitState, Unit>
    {
        private readonly IwsContext context;
        private readonly ITransportRouteRepository repository;

        public AddTransitStateHandler(IwsContext context, ITransportRouteRepository repository)
        {
            this.context = context;
            this.repository = repository;
        }

        public async Task<Unit> HandleAsync(AddTransitState message)
        {
            var transportRoute = await repository.GetByNotificationId(message.NotificationId);

            if (transportRoute == null)
            {
                transportRoute = new TransportRoute(message.NotificationId);
                context.TransportRoutes.Add(transportRoute);
            }

            var country = await context.Countries.SingleAsync(c => c.Id == message.CountryId);
            var competentAuthority =
                await context.CompetentAuthorities.SingleAsync(ca => ca.Id == message.CompetentAuthorityId);
            var entryPoint = await context.EntryOrExitPoints.SingleAsync(ep => ep.Id == message.EntryPointId);
            var exitPoint = await context.EntryOrExitPoints.SingleAsync(ep => ep.Id == message.ExitPointId);

            var ordinalPosition = transportRoute.GetAvailableTransitStatePositions().First();
            var transitState = new TransitState(country, competentAuthority, entryPoint, exitPoint, ordinalPosition);
            transportRoute.AddTransitStateToNotification(transitState);

            await context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}