namespace EA.Iws.RequestHandlers.StateOfImport
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.TransportRoute;
    using Prsd.Core.Mediator;
    using Requests.StateOfImport;

    internal class SetStateOfImportForNotificationHandler : IRequestHandler<SetStateOfImportForNotification, Guid>
    {
        private readonly IwsContext context;
        private readonly ITransportRouteRepository repository;
        private readonly IIntraCountryExportAllowedRepository iceaRepository;

        public SetStateOfImportForNotificationHandler(IwsContext context, ITransportRouteRepository repository, IIntraCountryExportAllowedRepository iceaRepository)
        {
            this.context = context;
            this.repository = repository;
            this.iceaRepository = iceaRepository;
        }

        public async Task<Guid> HandleAsync(SetStateOfImportForNotification message)
        {
            var transportRoute = await repository.GetByNotificationId(message.NotificationId);

            if (transportRoute == null)
            {
                transportRoute = new TransportRoute(message.NotificationId);
                context.TransportRoutes.Add(transportRoute);
            }

            var country = await context.Countries.SingleAsync(c => c.Id == message.CountryId);
            var competentAuthority =
                await context.CompetentAuthorities.SingleAsync(ca => ca.Id == message.CompetentAuthorityId);
            var entryPoint = await context.EntryOrExitPoints.SingleAsync(ep => ep.Id == message.EntryOrExitPointId);

            var stateOfImport = new StateOfImport(country, competentAuthority, entryPoint);

            var acceptableImportStates = await iceaRepository.GetAll();
            transportRoute.SetStateOfImportForNotification(stateOfImport, acceptableImportStates);

            await context.SaveChangesAsync();

            return stateOfImport.Id;
        }
    }
}
