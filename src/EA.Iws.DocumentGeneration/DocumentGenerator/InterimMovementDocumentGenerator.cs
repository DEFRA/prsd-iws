namespace EA.Iws.DocumentGeneration.DocumentGenerator
{
    using System;
    using System.Threading.Tasks;
    using DocumentFormat.OpenXml.Packaging;
    using Domain;
    using Domain.NotificationApplication;

    public class InterimMovementDocumentGenerator : IInterimMovementDocumentGenerator
    {
        private readonly INotificationApplicationRepository notificationApplicationRepository;

        public InterimMovementDocumentGenerator(INotificationApplicationRepository notificationApplicationRepository)
        {
            this.notificationApplicationRepository = notificationApplicationRepository;
        }

        public async Task<byte[]> Generate(Guid notificationId)
        {
            using (var memoryStream = DocumentHelper.ReadDocumentStreamShared("InterimMovementMergeTemplate.docx"))
            {
                using (var document = WordprocessingDocument.Open(memoryStream, true))
                {
                    var notificationNumber = await notificationApplicationRepository.GetNumber(notificationId);
                    MergeFieldLocator.MergeNamedField("NotificationNumber", notificationNumber, document);
                }

                return memoryStream.ToArray();
            }
        }
    }
}
