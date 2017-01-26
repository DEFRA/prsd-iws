namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using System.Threading.Tasks;
    using Core.StateOfImport;
    using Domain.ImportNotification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;

    internal class GetStateOfImportDataHandler : IRequestHandler<GetStateOfImportData, StateOfImportData>
    {
        private readonly IMapper mapper;
        private readonly ITransportRouteRepository transportRouteRepository;

        public GetStateOfImportDataHandler(ITransportRouteRepository transportRouteRepository,
            IMapper mapper)
        {
            this.transportRouteRepository = transportRouteRepository;
            this.mapper = mapper;
        }

        public async Task<StateOfImportData> HandleAsync(GetStateOfImportData message)
        {
            var transportRoute = await transportRouteRepository.GetByNotificationId(message.ImportNotificationId);
            return mapper.Map<StateOfImportData>(transportRoute.StateOfImport);
        }
    }
}