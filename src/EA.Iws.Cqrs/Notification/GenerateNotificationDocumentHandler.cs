namespace EA.Iws.Cqrs.Notification
{
    using System;
    using System.Data.Entity;
    using System.IO;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Prsd.Core.Mediator;

    public class GenerateNotificationDocumentHandler : IRequestHandler<GenerateNotificationDocument, byte[]>
    {
        private readonly IwsContext context;
        private readonly IDocumentGenerator documentGenerator;

        public GenerateNotificationDocumentHandler(IwsContext context, IDocumentGenerator documentGenerator)
        {
            this.context = context;
            this.documentGenerator = documentGenerator;
        }

        public async Task<byte[]> HandleAsync(GenerateNotificationDocument query)
        {
            // TODO - check if user has access to this notification
            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == query.NotificationId);

            return documentGenerator.GenerateNotificationDocument(notification, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Documents"));
        }
    }
}