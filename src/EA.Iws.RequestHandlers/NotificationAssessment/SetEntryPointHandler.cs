namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Data.Entity;
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
            IIntraCountryExportAllowedRepository intraCountryExportAllowedRepository,
            IwsContext context)
        {
            this.transportRouteRepository = transportRouteRepository;
            this.entryOrExitPointRepository = entryOrExitPointRepository;
            this.intraCountryExportAllowedRepository = intraCountryExportAllowedRepository;
            this.context = context;
        }

        public async Task<Unit> HandleAsync(SetEntryPoint message)
        {
            var transportRouteTask = transportRouteRepository.GetByNotificationId(message.NotificationId);
            var intraCountryExportAlloweds = intraCountryExportAllowedRepository.GetAll();
            var uksAuthorities = this.context.UnitedKingdomCompetentAuthorities.ToArrayAsync();
            var entryPoint = entryOrExitPointRepository.GetById(message.EntryPointId);
            var transportRoute = await transportRouteTask;
            var validator = new TransportRouteValidation(await intraCountryExportAlloweds, await uksAuthorities);

            transportRoute.SetStateOfImportForNotification(new StateOfImport(transportRoute.StateOfImport.Country,
                transportRoute.StateOfImport.CompetentAuthority, await entryPoint), validator);

            await context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}