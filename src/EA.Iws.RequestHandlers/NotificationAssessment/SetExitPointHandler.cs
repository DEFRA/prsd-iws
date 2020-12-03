namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Data.Entity;
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
            var intraCountryExportAlloweds = intraCountryExportAllowedRepository.GetAll();
            var uksAuthorities = this.context.UnitedKingdomCompetentAuthorities.ToArrayAsync();
            var transportRouteTask = transportRouteRepository.GetByNotificationId(message.NotificationId);
            var exitPoint = entryOrExitPointRepository.GetById(message.ExitPointId);
            var validator = new TransportRouteValidation(await intraCountryExportAlloweds, await uksAuthorities);
            var transportRoute = await transportRouteTask;

            transportRoute.SetStateOfExportForNotification(new StateOfExport(transportRoute.StateOfExport.Country,
                transportRoute.StateOfExport.CompetentAuthority, await exitPoint), validator);

            await context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}