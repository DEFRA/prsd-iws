namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Threading.Tasks;
    using Core.StateOfExport;
    using Domain.TransportRoute;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class GetStateOfExportDataHandler : IRequestHandler<GetStateOfExportData, StateOfExportData>
    {
        private readonly IMapper mapper;
        private readonly ITransportRouteRepository transportRouteRepository;

        public GetStateOfExportDataHandler(IMapper mapper,
            ITransportRouteRepository transportRouteRepository)
        {
            this.mapper = mapper;
            this.transportRouteRepository = transportRouteRepository;
        }

        public async Task<StateOfExportData> HandleAsync(GetStateOfExportData message)
        {
            var transportRoute = await transportRouteRepository.GetByNotificationId(message.NotificationId);
            return mapper.Map<StateOfExportData>(transportRoute.StateOfExport);
        }
    }
}