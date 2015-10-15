namespace EA.Iws.RequestHandlers.Notification
{
    using System.Threading.Tasks;
    using Core.Documents;
    using Documents;
    using Domain;
    using Prsd.Core;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GenerateNotificationDocumentHandler : IRequestHandler<GenerateNotificationDocument, FileData>
    {
        private readonly INotificationDocumentGenerator notificationDocumentGenerator;
        private readonly IPdfGenerator pdfGenerator;

        public GenerateNotificationDocumentHandler(INotificationDocumentGenerator notificationDocumentGenerator, IPdfGenerator pdfGenerator)
        {
            this.notificationDocumentGenerator = notificationDocumentGenerator;
            this.pdfGenerator = pdfGenerator;
        }

        public GenerateNotificationDocumentHandler(INotificationDocumentGenerator notificationDocumentGenerator)
        {
            this.notificationDocumentGenerator = notificationDocumentGenerator;
        }

        public async Task<FileData> HandleAsync(GenerateNotificationDocument query)
        {
            var docxBytes = await notificationDocumentGenerator.GenerateNotificationDocument(query.NotificationId);
            var fileName = string.Format("IWS-Notification-{0}-{1}", query.NotificationId, SystemTime.UtcNow);

            if (pdfGenerator != null)
            {
                var pdfBytes = pdfGenerator.ConvertToPdf(docxBytes);
                return new FileData(fileName, FileType.Pdf, pdfBytes);
            }

            return new FileData(fileName, FileType.Docx, docxBytes);
        }
    }
}