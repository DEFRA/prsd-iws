namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.TransitState;
    using Core.TransportRoute;
    using DataAccess;
    using Domain.TransportRoute;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class GetTransitStateWithEntryOrExitDataHandler : IRequestHandler<GetTransitStateWithEntryOrExitData, TransitStateWithEntryOrExitPointsData>
    {
        private readonly IwsContext context;
        private readonly IMapper mapper;
        private readonly ITransportRouteRepository transportRouteRepository;

        public GetTransitStateWithEntryOrExitDataHandler(IwsContext context,
            IMapper mapper,
            ITransportRouteRepository transportRouteRepository)
        {
            this.context = context;
            this.mapper = mapper;
            this.transportRouteRepository = transportRouteRepository;
        }

        public async Task<TransitStateWithEntryOrExitPointsData> HandleAsync(GetTransitStateWithEntryOrExitData message)
        {
            var transportRoute = await transportRouteRepository.GetByNotificationId(message.Id);
            var result = new TransitStateWithEntryOrExitPointsData()
            {
                TransitState = new TransitStateData(),
                EntryOrExitPoints = new List<EntryOrExitPointData>()
            };

            if (transportRoute == null)
            {
                return result;
            }

            var thisTransitState = transportRoute.TransitStates.SingleOrDefault(ts => ts.Id == message.TransitStateId);
            if (thisTransitState != null)
            {
                result.TransitState = mapper.Map<TransitStateData>(thisTransitState);
                result.EntryOrExitPoints =
                    (await
                            context.EntryOrExitPoints.Where(ep => ep.Country.Id == thisTransitState.Country.Id).ToArrayAsync())
                        .Select(e => mapper.Map<EntryOrExitPointData>(e));
            }

            return result;
        }
    }
}
