namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportNotification;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;

    internal class SetEntryPointHandler : IRequestHandler<SetEntryPoint, Unit>
    {
        private readonly ImportNotificationContext context;
        private readonly ITransportRouteRepository transportRouteRepository;

        public SetEntryPointHandler(ITransportRouteRepository transportRouteRepository,
            ImportNotificationContext context)
        {
            this.transportRouteRepository = transportRouteRepository;
            this.context = context;
        }

        public async Task<Unit> HandleAsync(SetEntryPoint message)
        {
            var transportRoute = await transportRouteRepository.GetByNotificationId(message.NotificationId);

            transportRoute.SetStateOfImport(new StateOfImport(message.EntryPointId,
                transportRoute.StateOfImport.CompetentAuthorityId));

            await context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}