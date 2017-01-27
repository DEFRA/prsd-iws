namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.TransportRoute;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class SetEntryPointHandler : IRequestHandler<SetEntryPoint, Unit>
    {
        private readonly IEntryOrExitPointRepository entryOrExitPointRepository;
        private readonly IwsContext context;
        private readonly ITransportRouteRepository transportRouteRepository;

        public SetEntryPointHandler(ITransportRouteRepository transportRouteRepository,
            IEntryOrExitPointRepository entryOrExitPointRepository,
            IwsContext context)
        {
            this.transportRouteRepository = transportRouteRepository;
            this.entryOrExitPointRepository = entryOrExitPointRepository;
            this.context = context;
        }

        public async Task<Unit> HandleAsync(SetEntryPoint message)
        {
            var transportRoute = await transportRouteRepository.GetByNotificationId(message.NotificationId);
            var entryPoint = await entryOrExitPointRepository.GetById(message.EntryPointId);

            transportRoute.SetStateOfImportForNotification(new StateOfImport(transportRoute.StateOfImport.Country,
                transportRoute.StateOfImport.CompetentAuthority, entryPoint));

            await context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}