namespace EA.Iws.RequestHandlers.Exporters
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Exporters;

    internal class NotificationHasExporterHandler : IRequestHandler<NotificationHasExporter, bool>
    {
        private readonly IwsContext context;

        public NotificationHasExporterHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<bool> HandleAsync(NotificationHasExporter message)
        {
            var notification = await context.NotificationApplications.SingleAsync(p => p.Id == message.NotificationId);
            return notification.HasExporter;
        }
    }
}