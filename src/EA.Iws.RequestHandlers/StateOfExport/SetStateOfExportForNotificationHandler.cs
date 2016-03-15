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

        public SetStateOfExportForNotificationHandler(IwsContext context, ITransportRouteRepository transportRouteRepository)
        {
            this.context = context;
            this.transportRouteRepository = transportRouteRepository;
        }

        public async Task<Guid> HandleAsync(SetStateOfExportForNotification message)
        {
            var notification = await context.GetNotificationApplication(message.NotificationId);
            var transportRoute = await transportRouteRepository.GetByNotificationId(message.NotificationId);

            if (transportRoute == null)
            {
                transportRoute = new TransportRoute(message.NotificationId);
                context.TransportRoutes.Add(transportRoute);
            }

            var ukcompAuth = await context.UnitedKingdomCompetentAuthorities.SingleAsync(ca => ca.Id == (int)notification.CompetentAuthority);

            var country = await context.Countries.SingleAsync(c => c.Name == UnitedKingdomCompetentAuthority.CountryName);

            var caid = ukcompAuth.CompetentAuthority.Id;
            var competentAuthority = await context.CompetentAuthorities.SingleAsync(ca => ca.Id == caid);

            var exitPoint = await context.EntryOrExitPoints.SingleAsync(ep => ep.Id == message.EntryOrExitPointId);

            var stateOfExport = new StateOfExport(country, competentAuthority, exitPoint);

            transportRoute.SetStateOfExportForNotification(stateOfExport);

            await context.SaveChangesAsync();

            return stateOfExport.Id;
        }
    }
}
