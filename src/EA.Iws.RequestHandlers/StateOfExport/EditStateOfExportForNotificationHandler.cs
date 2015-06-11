namespace EA.Iws.RequestHandlers.StateOfExport
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.StateOfExport;

    internal class EditStateOfExportForNotificationHandler : IRequestHandler<EditStateOfExportForNotification, Guid>
    {
        private readonly IwsContext context;

        public EditStateOfExportForNotificationHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(EditStateOfExportForNotification message)
        {
            var notification = await context.NotificationApplications.SingleAsync(na => na.Id == message.NotificationId);

            var country = await context.Countries.SingleAsync(c => c.Id == message.CountryId);
            var competentAuthority =
                await context.CompetentAuthorities.SingleAsync(ca => ca.Id == message.CompetentAuthorityId);
            var exitPoint = await context.EntryOrExitPoints.SingleAsync(ep => ep.Id == message.EntryOrExitPointId);

            notification.UpdateStateOfExport(country, competentAuthority, exitPoint);

            await context.SaveChangesAsync();

            return notification.StateOfExport.Id;
        }
    }
}
