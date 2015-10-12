namespace EA.Iws.RequestHandlers.StateOfExport
{
    using System;
    using System.Threading.Tasks;
    using Core.StateOfExport;
    using Domain.TransportRoute;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.StateOfExport;

    internal class GetStateOfExportWithTransportRouteDataByNotificationIdHandler :
        IRequestHandler<GetStateOfExportWithTransportRouteDataByNotificationId, StateOfExportWithTransportRouteData>
    {
        private readonly ITransportRouteRepository repository;
        private readonly IMapWithParameter<TransportRoute, Guid, StateOfExportWithTransportRouteData> transportRouteMap;

        public GetStateOfExportWithTransportRouteDataByNotificationIdHandler(ITransportRouteRepository repository,
            IMapWithParameter<TransportRoute, Guid, StateOfExportWithTransportRouteData> transportRouteMap)
        {
            this.repository = repository;
            this.transportRouteMap = transportRouteMap;
        }

        public async Task<StateOfExportWithTransportRouteData> HandleAsync(
            GetStateOfExportWithTransportRouteDataByNotificationId message)
        {
            var transportRoute = await repository.GetByNotificationId(message.Id);

            return transportRouteMap.Map(transportRoute, message.Id);
        }
    }
}