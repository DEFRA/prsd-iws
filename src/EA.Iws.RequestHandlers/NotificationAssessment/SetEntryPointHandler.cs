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
        private readonly IUnitedKingdomCompetentAuthorityRepository unitedKingdomCompetentAuthorityRepository;

        public SetEntryPointHandler(ITransportRouteRepository transportRouteRepository,
            IEntryOrExitPointRepository entryOrExitPointRepository,
            IIntraCountryExportAllowedRepository intraCountryExportAllowedRepository,
            IUnitedKingdomCompetentAuthorityRepository unitedKingdomCompetentAuthorityRepository,
            IwsContext context)
        {
            this.transportRouteRepository = transportRouteRepository;
            this.entryOrExitPointRepository = entryOrExitPointRepository;
            this.intraCountryExportAllowedRepository = intraCountryExportAllowedRepository;
            this.unitedKingdomCompetentAuthorityRepository = unitedKingdomCompetentAuthorityRepository;
            this.context = context;
        }

        public async Task<Unit> HandleAsync(SetEntryPoint message)
        {
            var intraCountryExportAlloweds = await intraCountryExportAllowedRepository.GetAllAsync();
            var uksAuthorities = await unitedKingdomCompetentAuthorityRepository.GetAllAsync();
            var entryPoint = await entryOrExitPointRepository.GetById(message.EntryPointId);
            var transportRoute = await transportRouteRepository.GetByNotificationId(message.NotificationId);
            var validator = new TransportRouteValidation(intraCountryExportAlloweds, uksAuthorities);

            transportRoute.SetStateOfImportForNotification(new StateOfImport(transportRoute.StateOfImport.Country,
                transportRoute.StateOfImport.CompetentAuthority, entryPoint), validator);

            await context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}