namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using System.Threading.Tasks;
    using Core.StateOfExport;
    using Domain.ImportNotification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;

    internal class GetStateOfExportDataHandler : IRequestHandler<GetStateOfExportData, StateOfExportData>
    {
        private readonly IMapper mapper;
        private readonly ITransportRouteRepository transportRouteRepository;

        public GetStateOfExportDataHandler(ITransportRouteRepository transportRouteRepository,
            IMapper mapper)
        {
            this.transportRouteRepository = transportRouteRepository;
            this.mapper = mapper;
        }

        public async Task<StateOfExportData> HandleAsync(GetStateOfExportData message)
        {
            var transportRoute = await transportRouteRepository.GetByNotificationId(message.ImportNotificationId);
            return mapper.Map<StateOfExportData>(transportRoute.StateOfExport);
        }
    }
}