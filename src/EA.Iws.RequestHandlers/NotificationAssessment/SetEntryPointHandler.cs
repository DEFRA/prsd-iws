namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.TransportRoute;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class SetEntryPointHandler : IRequestHandler<SetEntryPoint, Unit>
    {
        private readonly IEntryOrExitPointRepository entryOrExitPointRepository;
        private readonly IwsContext context;
        private readonly ITransportRouteRepository transportRouteRepository;
        private readonly IIntraCountryExportAllowedRepository intraCountryExportAllowedRepository;

        public SetEntryPointHandler(ITransportRouteRepository transportRouteRepository,
            IEntryOrExitPointRepository entryOrExitPointRepository,
            IwsContext context)
        {
            this.transportRouteRepository = transportRouteRepository;
            this.entryOrExitPointRepository = entryOrExitPointRepository;
            this.intraCountryExportAllowedRepository = intraCountryExportAllowedRepository;
            this.context = context;
        }

        public async Task<Unit> HandleAsync(SetEntryPoint message)
        {
            var transportRoute = await transportRouteRepository.GetByNotificationId(message.NotificationId);
            var entryPoint = await entryOrExitPointRepository.GetById(message.EntryPointId);
            var intraCountryExportAlloweds = await intraCountryExportAllowedRepository.GetAll();

            transportRoute.SetStateOfImportForNotification(new StateOfImport(transportRoute.StateOfImport.Country,
                transportRoute.StateOfImport.CompetentAuthority, entryPoint), intraCountryExportAlloweds);

            await context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}