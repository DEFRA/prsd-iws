namespace EA.Iws.RequestHandlers.TransportRoute
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.StateOfExport;
    using Core.StateOfImport;
    using Core.TransitState;
    using Core.TransportRoute;
    using Domain.TransportRoute;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.TransportRoute;

    internal class GetTransportRouteSummaryForNotificationHandler : IRequestHandler<GetTransportRouteSummaryForNotification, TransportRouteData>
    {
        private readonly ITransportRouteRepository repository;
        private readonly IMap<StateOfExport, StateOfExportData> stateOfExportMapper;
        private readonly IMap<StateOfImport, StateOfImportData> stateOfImportMapper;
        private readonly IMap<IEnumerable<TransitState>, IList<TransitStateData>> transitStateMapper;

        public GetTransportRouteSummaryForNotificationHandler(ITransportRouteRepository repository,
            IMap<StateOfExport, StateOfExportData> stateOfExportMapper,
            IMap<StateOfImport, StateOfImportData> stateOfImportMapper,
            IMap<IEnumerable<TransitState>, IList<TransitStateData>> transitStateMapper)
        {
            this.repository = repository;
            this.stateOfExportMapper = stateOfExportMapper;
            this.stateOfImportMapper = stateOfImportMapper;
            this.transitStateMapper = transitStateMapper;
        }

        public async Task<TransportRouteData> HandleAsync(GetTransportRouteSummaryForNotification message)
        {
            var transportRoute = await repository.GetByNotificationId(message.NotificationId);

            return new TransportRouteData
            {
                StateOfExportData = transportRoute == null ? null : stateOfExportMapper.Map(transportRoute.StateOfExport),
                StateOfImportData = transportRoute == null ? null : stateOfImportMapper.Map(transportRoute.StateOfImport),
                TransitStatesData = transportRoute == null ? null : transitStateMapper.Map(transportRoute.TransitStates)
            };
        }
    }
}
