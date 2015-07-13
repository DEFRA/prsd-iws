namespace EA.Iws.RequestHandlers.Notification
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GenerateNotificationDocumentHandler : IRequestHandler<GenerateNotificationDocument, byte[]>
    {
        private readonly IwsContext context;
        private readonly IDocumentGenerator documentGenerator;

        public GenerateNotificationDocumentHandler(IwsContext context,
            IDocumentGenerator documentGenerator)
        {
            this.context = context;
            this.documentGenerator = documentGenerator;
        }

        public async Task<byte[]> HandleAsync(GenerateNotificationDocument query)
        {
            var notification = await context.GetNotificationApplication(query.NotificationId);

            return documentGenerator.GenerateNotificationDocument(notification);
        }
    }
}