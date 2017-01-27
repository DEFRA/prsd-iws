namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportNotification;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;

    internal class SetExitPointHandler : IRequestHandler<SetExitPoint, Unit>
    {
        private readonly ImportNotificationContext context;
        private readonly ITransportRouteRepository transportRouteRepository;

        public SetExitPointHandler(ITransportRouteRepository transportRouteRepository,
            ImportNotificationContext context)
        {
            this.transportRouteRepository = transportRouteRepository;
            this.context = context;
        }

        public async Task<Unit> HandleAsync(SetExitPoint message)
        {
            var transportRoute = await transportRouteRepository.GetByNotificationId(message.NotificationId);

            transportRoute.SetStateOfExport(new StateOfExport(message.ExitPointId,
                transportRoute.StateOfExport.CompetentAuthorityId, transportRoute.StateOfExport.CountryId));

            await context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}