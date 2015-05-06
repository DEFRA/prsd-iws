namespace EA.Iws.DocumentGeneration.DocumentGenerator
{
    using System.IO;
    using Core.Cqrs;
    using DocumentFormat.OpenXml.Packaging;
    using Domain;
    using Domain.Notification;

    public class DocumentGenerator : IDocumentGenerator
    {
        private readonly NotificationDocumentMerger notificationDocumentMerger;
        private readonly IQueryBus queryBus;

        public DocumentGenerator(IQueryBus queryBus,
            NotificationDocumentMerger notificationDocumentMerger)
        {
            this.queryBus = queryBus;
            this.notificationDocumentMerger = notificationDocumentMerger;
        }

        public byte[] GenerateNotificationDocument(NotificationApplication notification, string applicationDirectory)
        {
            return GenerateMainDocument(notification, applicationDirectory);
        }

        private byte[] GenerateMainDocument(NotificationApplication notification, string applicationDirectory)
        {
            var pathToTemplate = applicationDirectory + "NotificationMergeTemplate.docx";

            // Minimise time the process is using the template file to prevent contention between processes.
            var templateFile = DocumentHelper.ReadDocumentShared(pathToTemplate);

            using (var memoryStream = new MemoryStream())
            {
                memoryStream.Write(templateFile, 0, templateFile.Length);

                using (var document = WordprocessingDocument.Open(memoryStream, true))
                {
                    var mergeFields = MergeFieldLocator.GetMergeRuns(document);

                    notificationDocumentMerger.MergeDataIntoDocument(mergeFields, notification);

                    MergeFieldLocator.RemoveDataSourceSettingFromMergedDocument(document);
                }

                return memoryStream.ToArray();
            }
        }
    }
}