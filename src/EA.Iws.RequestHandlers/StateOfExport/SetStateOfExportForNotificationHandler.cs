namespace EA.Iws.RequestHandlers.StateOfExport
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.TransportRoute;
    using Prsd.Core.Mediator;
    using Requests.StateOfExport;

    internal class SetStateOfExportForNotificationHandler : IRequestHandler<SetStateOfExportForNotification, Guid>
    {
        private readonly IwsContext context;

        public SetStateOfExportForNotificationHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(SetStateOfExportForNotification message)
        {
            var notification = await context.GetNotificationApplication(message.NotificationId);
            var transportRoute = await context.TransportRoutes.SingleOrDefaultAsync(p => p.NotificationId == message.NotificationId);

            if (transportRoute == null)
            {
                transportRoute = new TransportRoute(message.NotificationId);
                context.TransportRoutes.Add(transportRoute);
            }

            var ukcompAuth = await context.UnitedKingdomCompetentAuthorities.SingleAsync(ca => ca.Id == notification.CompetentAuthority.Value);

            var country = await context.Countries.SingleAsync(c => c.Name == ukcompAuth.CountryName);

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
