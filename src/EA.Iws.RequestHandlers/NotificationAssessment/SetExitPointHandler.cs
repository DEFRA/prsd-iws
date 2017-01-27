namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.TransportRoute;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class SetExitPointHandler : IRequestHandler<SetExitPoint, Unit>
    {
        private readonly IwsContext context;
        private readonly IEntryOrExitPointRepository entryOrExitPointRepository;
        private readonly ITransportRouteRepository transportRouteRepository;

        public SetExitPointHandler(ITransportRouteRepository transportRouteRepository,
            IEntryOrExitPointRepository entryOrExitPointRepository,
            IwsContext context)
        {
            this.transportRouteRepository = transportRouteRepository;
            this.entryOrExitPointRepository = entryOrExitPointRepository;
            this.context = context;
        }

        public async Task<Unit> HandleAsync(SetExitPoint message)
        {
            var transportRoute = await transportRouteRepository.GetByNotificationId(message.NotificationId);
            var exitPoint = await entryOrExitPointRepository.GetById(message.ExitPointId);

            transportRoute.SetStateOfExportForNotification(new StateOfExport(transportRoute.StateOfExport.Country,
                transportRoute.StateOfExport.CompetentAuthority, exitPoint));

            await context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}