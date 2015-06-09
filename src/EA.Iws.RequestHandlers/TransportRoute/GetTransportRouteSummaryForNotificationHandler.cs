namespace EA.Iws.RequestHandlers.TransportRoute
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.TransportRoute;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.StateOfExport;
    using Requests.StateOfImport;
    using Requests.TransportRoute;

    internal class GetTransportRouteSummaryForNotificationHandler : IRequestHandler<GetTransportRouteSummaryForNotification, TransportRouteData>
    {
        private readonly IwsContext context;
        private readonly IMap<StateOfExport, StateOfExportData> stateOfExportMapper;
        private readonly IMap<StateOfImport, StateOfImportData> stateOfImportMapper;
        private readonly IMap<IEnumerable<TransitState>, IList<TransitStateData>> transitStateMapper;

        public GetTransportRouteSummaryForNotificationHandler(IwsContext context, 
            IMap<StateOfExport, StateOfExportData> stateOfExportMapper, 
            IMap<StateOfImport, StateOfImportData> stateOfImportMapper,
            IMap<IEnumerable<TransitState>, IList<TransitStateData>> transitStateMapper)
        {
            this.context = context;
            this.stateOfExportMapper = stateOfExportMapper;
            this.stateOfImportMapper = stateOfImportMapper;
            this.transitStateMapper = transitStateMapper;
        }

        public async Task<TransportRouteData> HandleAsync(GetTransportRouteSummaryForNotification message)
        {
            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == message.NotificationId);

            return new TransportRouteData
            {
                StateOfExportData = stateOfExportMapper.Map(notification.StateOfExport),
                StateOfImportData = stateOfImportMapper.Map(notification.StateOfImport),
                TransitStatesData = transitStateMapper.Map(notification.TransitStates)
            };
        }
    }
}
