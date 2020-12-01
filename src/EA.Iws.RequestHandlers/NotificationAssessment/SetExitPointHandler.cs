namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.TransportRoute;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class SetExitPointHandler : IRequestHandler<SetExitPoint, Unit>
    {
        private readonly IwsContext context;
        private readonly IEntryOrExitPointRepository entryOrExitPointRepository;
        private readonly ITransportRouteRepository transportRouteRepository;
        private readonly IIntraCountryExportAllowedRepository intraCountryExportAllowedRepository;

        public SetExitPointHandler(ITransportRouteRepository transportRouteRepository,
            IEntryOrExitPointRepository entryOrExitPointRepository,
            IIntraCountryExportAllowedRepository intraCountryExportAllowedRepository,
            IwsContext context)
        {
            this.transportRouteRepository = transportRouteRepository;
            this.entryOrExitPointRepository = entryOrExitPointRepository;
            this.intraCountryExportAllowedRepository = intraCountryExportAllowedRepository;
            this.context = context;
        }

        public async Task<Unit> HandleAsync(SetExitPoint message)
        {
            var transportRoute = await transportRouteRepository.GetByNotificationId(message.NotificationId);
            var exitPoint = await entryOrExitPointRepository.GetById(message.ExitPointId);
            var intraCountryExportAlloweds = await intraCountryExportAllowedRepository.GetAll();

            transportRoute.SetStateOfExportForNotification(new StateOfExport(transportRoute.StateOfExport.Country,
                transportRoute.StateOfExport.CompetentAuthority, exitPoint), intraCountryExportAlloweds);

            await context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}