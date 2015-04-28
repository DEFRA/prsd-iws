namespace EA.Iws.Cqrs.Notification
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.Cqrs;
    using DataAccess;
    using Domain;

    public class GenerateNotificationDocumentHandler : IQueryHandler<GenerateNotificationDocument, byte[]>
    {
        private readonly IwsContext context;
        private readonly IDocumentGenerator documentGenerator;

        public GenerateNotificationDocumentHandler(IwsContext context, IDocumentGenerator documentGenerator)
        {
            this.context = context;
            this.documentGenerator = documentGenerator;
        }

        public async Task<byte[]> ExecuteAsync(GenerateNotificationDocument query)
        {
            // TODO - check if user has access to this notification
            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == query.NotificationId);

            return documentGenerator.GenerateNotificationDocument(notification);
        }
    }
}