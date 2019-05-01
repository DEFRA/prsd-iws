namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.TransportRoute;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class UpdateTransitStateEntryOrExitHandler : IRequestHandler<UpdateTransitStateEntryOrExit, Guid>
    {
        private readonly IwsContext context;
        private readonly ITransportRouteRepository transportRouteRepository;

        public UpdateTransitStateEntryOrExitHandler(IwsContext context, ITransportRouteRepository transportRouteRepository)
        {
            this.context = context;
            this.transportRouteRepository = transportRouteRepository;
        }

        public async Task<Guid> HandleAsync(UpdateTransitStateEntryOrExit message)
        {
            var transportRoute = await transportRouteRepository.GetByNotificationId(message.NotificationId);

            if (transportRoute == null)
            {
                return Guid.Empty;
            }

            var transitState = transportRoute.TransitStates.SingleOrDefault(ts => ts.Id == message.TransitStateId);

            if (transitState == null)
            {
                return Guid.Empty;
            }

            transportRoute.UpdateTransitStateEntryOrExitPoint(transitState.Id,
                !message.EntryPointId.HasValue
                    ? transitState.EntryPoint
                    : await context.EntryOrExitPoints.SingleAsync(ep => ep.Id == message.EntryPointId),
                !message.ExitPointId.HasValue
                    ? transitState.ExitPoint
                    : await context.EntryOrExitPoints.SingleAsync(ep => ep.Id == message.ExitPointId));

            await context.SaveChangesAsync();

            return message.TransitStateId;
        }
    }
}
