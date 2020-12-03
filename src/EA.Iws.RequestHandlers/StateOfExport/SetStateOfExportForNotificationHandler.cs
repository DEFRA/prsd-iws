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
            var notificationTask = context.GetNotificationApplication(message.NotificationId);
            var transportRoute = await transportRouteRepository.GetByNotificationId(message.NotificationId);
            var country = context.Countries.SingleAsync(c => c.Name == UnitedKingdomCompetentAuthority.CountryName);
            var exitPoint = context.EntryOrExitPoints.SingleAsync(ep => ep.Id == message.EntryOrExitPointId);
            var acceptableExportStates = iceaRepository.GetAll();
            var unitedKingdomAuthorities = this.context.UnitedKingdomCompetentAuthorities.ToArrayAsync();

            if (transportRoute == null)
            {
                transportRoute = new TransportRoute(message.NotificationId);
                context.TransportRoutes.Add(transportRoute);
            }

            var notification = await notificationTask;
            var ukcompAuth = await context.UnitedKingdomCompetentAuthorities.SingleAsync(ca => ca.Id == (int)notification.CompetentAuthority);
            var caid = ukcompAuth.CompetentAuthority.Id;
            var competentAuthority = context.CompetentAuthorities.SingleAsync(ca => ca.Id == caid);
            var stateOfExport = new StateOfExport(await country, await competentAuthority, await exitPoint);
            var validator = new TransportRouteValidation(await acceptableExportStates, await unitedKingdomAuthorities);

            transportRoute.SetStateOfExportForNotification(stateOfExport, validator);

            await context.SaveChangesAsync();

            return stateOfExport.Id;
        }
    }
}
