namespace EA.Iws.RequestHandlers.TransportRoute
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.TransportRoute;
    using Prsd.Core.Mediator;
    using Requests.TransportRoute;

    internal class AddStateOfExportToNotificationHandler : IRequestHandler<AddStateOfExportToNotification, Guid>
    {
        private readonly IwsContext context;

        public AddStateOfExportToNotificationHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(AddStateOfExportToNotification message)
        {
            var notification = await context.NotificationApplications.SingleAsync(na => na.Id == message.NotificationId);

            var country = await context.Countries.SingleAsync(c => c.Id == message.CountryId);
            var competentAuthority =
                await context.CompetentAuthorities.SingleAsync(ca => ca.Id == message.CompetentAuthorityId);
            var exitPoint = await context.EntryOrExitPoints.SingleAsync(ep => ep.Id == message.EntryOrExitPointId);

            var stateOfExport = new StateOfExport(country, competentAuthority, exitPoint);

            notification.AddStateOfExportToNotification(stateOfExport);

            await context.SaveChangesAsync();

            return stateOfExport.Id;
        }
    }
}
