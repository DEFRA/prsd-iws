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

            var country = await context.Countries.SingleAsync(c => c.Id == message.CountryId);
            var competentAuthority =
                await context.CompetentAuthorities.SingleAsync(ca => ca.Id == message.CompetentAuthorityId);
            var exitPoint = await context.EntryOrExitPoints.SingleAsync(ep => ep.Id == message.EntryOrExitPointId);

            var stateOfExport = new StateOfExport(country, competentAuthority, exitPoint);

            notification.SetStateOfExportForNotification(stateOfExport);

            await context.SaveChangesAsync();

            return stateOfExport.Id;
        }
    }
}
