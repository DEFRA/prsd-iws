namespace EA.Iws.RequestHandlers.StateOfExport
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.TransportRoute;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.StateOfExport;

    internal class GetStateOfExportWithTransportRouteDataByNotificationIdHandler :
        IRequestHandler<GetStateOfExportWithTransportRouteDataByNotificationId, StateOfExportWithTransportRouteData>
    {
        private readonly IwsContext context;
        private readonly IMap<TransportRoute, StateOfExportWithTransportRouteData> transportRouteMap;

        public GetStateOfExportWithTransportRouteDataByNotificationIdHandler(IwsContext context,
            IMap<TransportRoute, StateOfExportWithTransportRouteData> transportRouteMap)
        {
            this.context = context;
            this.transportRouteMap = transportRouteMap;
        }

        public async Task<StateOfExportWithTransportRouteData> HandleAsync(
            GetStateOfExportWithTransportRouteDataByNotificationId message)
        {
            await context.CheckNotificationAccess(message.Id);

            var transportRoute = await context.TransportRoutes.SingleAsync(p => p.NotificationId == message.Id);

            return transportRouteMap.Map(transportRoute);
        }
    }
}