namespace EA.Iws.RequestHandlers.Notification
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GetReasonForExportHandler : IRequestHandler<GetReasonForExport, string>
    {
        private readonly IwsContext context;

        public GetReasonForExportHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<string> HandleAsync(GetReasonForExport message)
        {
            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == message.NotificationId);

            return notification.ReasonForExport;
        }
    }
}