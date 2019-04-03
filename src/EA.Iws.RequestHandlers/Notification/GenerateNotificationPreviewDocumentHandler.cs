namespace EA.Iws.RequestHandlers.Notification
{
    using Core.Documents;
    using Documents;
    using Domain;
    using Prsd.Core;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using System.Threading.Tasks;

    internal class GenerateNotificationPreviewDocumentHandler : IRequestHandler<GenerateNotificationPreviewDocument, FileData>
    {
        private readonly INotificationDocumentGenerator notificationDocumentGenerator;
        private readonly INotificationPreviewWatermarkGenerator notificationPreviewWatermarkGenerator;
        private readonly IPdfGenerator pdfGenerator;

        public GenerateNotificationPreviewDocumentHandler(INotificationDocumentGenerator notificationDocumentGenerator, INotificationPreviewWatermarkGenerator notificationPreviewWatermarkGenerator)
        {
            this.notificationDocumentGenerator = notificationDocumentGenerator;
            this.notificationPreviewWatermarkGenerator = notificationPreviewWatermarkGenerator;
        }

        public GenerateNotificationPreviewDocumentHandler(INotificationDocumentGenerator notificationDocumentGenerator, INotificationPreviewWatermarkGenerator notificationPreviewWatermarkGenerator, IPdfGenerator pdfGenerator)
        {
            this.notificationDocumentGenerator = notificationDocumentGenerator;
            this.notificationPreviewWatermarkGenerator = notificationPreviewWatermarkGenerator;
            this.pdfGenerator = pdfGenerator;
        }

        public async Task<FileData> HandleAsync(GenerateNotificationPreviewDocument query)
        {
            var docxBytes = await notificationDocumentGenerator.GenerateNotificationDocument(query.NotificationId);
            var fileName = string.Format("IWS-Notification-Preview-{0}-{1}", query.NotificationId, SystemTime.UtcNow);

            if (pdfGenerator == null)
            {
                var wordBytes = notificationPreviewWatermarkGenerator.GenerateNotificationPreviewWatermark(docxBytes);
                return new FileData(fileName, FileType.Docx, wordBytes);
            }

            var pdfBytes = pdfGenerator.ConvertToPreviewPdf(docxBytes);
            return new FileData(fileName, FileType.Pdf, pdfBytes);
        }
    }
}