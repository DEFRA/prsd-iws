namespace EA.Iws.RequestHandlers.TransitState
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.TransportRoute;
    using Prsd.Core.Mediator;
    using Requests.TransitState;

    internal class SetTransitStateForNotificationHandler : IRequestHandler<SetTransitStateForNotification, Guid>
    {
        private readonly IwsContext context;
        private readonly ITransportRouteRepository repository;

        public SetTransitStateForNotificationHandler(IwsContext context, ITransportRouteRepository repository)
        {
            this.context = context;
            this.repository = repository;
        }

        public async Task<Guid> HandleAsync(SetTransitStateForNotification message)
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

            Guid result;
            if (message.TransitStateId.HasValue)
            {
                transportRoute.UpdateTransitStateForNotification(message.TransitStateId.Value,
                    country, 
                    competentAuthority, 
                    entryPoint, 
                    exitPoint, 
                    message.OrdinalPosition);

                result = message.TransitStateId.Value;
            }
            else
            {
                var ordinalPosition = transportRoute.GetAvailableTransitStatePositions().First();
                var transitState = new TransitState(country, competentAuthority, entryPoint, exitPoint, ordinalPosition);
                result = transitState.Id;
                transportRoute.AddTransitStateToNotification(transitState);
            }

            await context.SaveChangesAsync();

            return result;
        }
    }
}
