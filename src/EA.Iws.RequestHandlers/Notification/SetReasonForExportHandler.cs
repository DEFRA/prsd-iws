namespace EA.Iws.RequestHandlers.Notification
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    public class SetReasonForExportHandler : IRequestHandler<SetReasonForExport, string>
    {
        private readonly IwsContext context;

        public SetReasonForExportHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<string> HandleAsync(SetReasonForExport query)
        {
            var notification = await context.NotificationApplications.Include(n => n.ShipmentInfo).SingleAsync(n => n.Id == query.NotificationId);
            notification.ReasonForExport = query.ReasonForExport;

            await context.SaveChangesAsync();

            return notification.NotificationNumber;
        }
    }
}