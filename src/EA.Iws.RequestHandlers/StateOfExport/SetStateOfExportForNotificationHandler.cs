namespace EA.Iws.RequestHandlers.StateOfExport
{
    using DataAccess;
    using Domain;
    using Domain.TransportRoute;
    using Prsd.Core.Mediator;
    using Requests.StateOfExport;
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;

    internal class SetStateOfExportForNotificationHandler : IRequestHandler<SetStateOfExportForNotification, Guid>
    {
        private readonly IwsContext context;
        private readonly ITransportRouteRepository transportRouteRepository;
        private readonly IIntraCountryExportAllowedRepository iceaRepository;

        public SetStateOfExportForNotificationHandler(IwsContext context, ITransportRouteRepository transportRouteRepository, IIntraCountryExportAllowedRepository iceaRepository)
        {
            this.context = context;
            this.transportRouteRepository = transportRouteRepository;
            this.iceaRepository = iceaRepository;
        }

        public async Task<Guid> HandleAsync(SetStateOfExportForNotification message)
        {
            var notification = await context.GetNotificationApplication(message.NotificationId);
            var transportRoute = await transportRouteRepository.GetByNotificationId(message.NotificationId);
            var country = await context.Countries.SingleAsync(c => c.Name == UnitedKingdomCompetentAuthority.CountryName);
            var exitPoint = await context.EntryOrExitPoints.SingleAsync(ep => ep.Id == message.EntryOrExitPointId);
            var acceptableExportStates = await iceaRepository.GetAll();
            var unitedKingdomAuthorities = await context.UnitedKingdomCompetentAuthorities.ToArrayAsync();

            if (transportRoute == null)
            {
                transportRoute = new TransportRoute(message.NotificationId);
                context.TransportRoutes.Add(transportRoute);
            }

            var ukcompAuth = await context.UnitedKingdomCompetentAuthorities.SingleAsync(ca => ca.Id == (int)notification.CompetentAuthority);
            var caid = ukcompAuth.CompetentAuthority.Id;
            var competentAuthority = await context.CompetentAuthorities.SingleAsync(ca => ca.Id == caid);
            var stateOfExport = new StateOfExport(country, competentAuthority, exitPoint);
            var validator = new TransportRouteValidation(acceptableExportStates, unitedKingdomAuthorities);

            transportRoute.SetStateOfExportForNotification(stateOfExport, validator);

            await context.SaveChangesAsync();

            return stateOfExport.Id;
        }
    }
}
