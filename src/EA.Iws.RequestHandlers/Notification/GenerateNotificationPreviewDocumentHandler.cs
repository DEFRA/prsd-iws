namespace EA.Iws.RequestHandlers.Notification
{
    using System.Threading.Tasks;
    using Core.Documents;
    using Documents;
    using Domain;
    using Prsd.Core;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GenerateNotificationPreviewDocumentHandler : IRequestHandler<GenerateNotificationPreviewDocument, FileData>
    {
        private readonly INotificationDocumentGenerator notificationDocumentGenerator;
        private readonly IPdfGenerator pdfGenerator;

        public GenerateNotificationPreviewDocumentHandler(INotificationDocumentGenerator notificationDocumentGenerator, IPdfGenerator pdfGenerator)
        {
            this.notificationDocumentGenerator = notificationDocumentGenerator;
            this.pdfGenerator = pdfGenerator;
        }

        public async Task<FileData> HandleAsync(GenerateNotificationPreviewDocument query)
        {
            var docxBytes = await notificationDocumentGenerator.GenerateNotificationDocument(query.NotificationId);
            var fileName = string.Format("IWS-Notification-Preview-{0}-{1}", query.NotificationId, SystemTime.UtcNow);

            var pdfBytes = pdfGenerator.ConvertToPreviewPdf(docxBytes);
            return new FileData(fileName, FileType.Pdf, pdfBytes);
        }
    }
}