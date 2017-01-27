namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Threading.Tasks;
    using Core.StateOfImport;
    using Domain.TransportRoute;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class GetStateOfImportDataHandler : IRequestHandler<GetStateOfImportData, StateOfImportData>
    {
        private readonly IMapper mapper;
        private readonly ITransportRouteRepository transportRouteRepository;

        public GetStateOfImportDataHandler(IMapper mapper,
            ITransportRouteRepository transportRouteRepository)
        {
            this.mapper = mapper;
            this.transportRouteRepository = transportRouteRepository;
        }

        public async Task<StateOfImportData> HandleAsync(GetStateOfImportData message)
        {
            var transportRoute = await transportRouteRepository.GetByNotificationId(message.NotificationId);
            return mapper.Map<StateOfImportData>(transportRoute.StateOfImport);
        }
    }
}