namespace EA.Iws.RequestHandlers.Importers
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Importer;

    internal class NotificationHasImporterHandler : IRequestHandler<NotificationHasImporter, bool>
    {
        private readonly IwsContext context;

        public NotificationHasImporterHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<bool> HandleAsync(NotificationHasImporter message)
        {
            var notification = await context.NotificationApplications.SingleAsync(p => p.Id == message.NotificationId);
            return notification.HasImporter;
        }
    }
}