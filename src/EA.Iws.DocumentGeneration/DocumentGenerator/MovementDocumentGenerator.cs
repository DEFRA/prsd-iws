namespace EA.Iws.DocumentGeneration.DocumentGenerator
{
    using DocumentFormat.OpenXml.Packaging;
    using Domain;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using Movement;

    public class MovementDocumentGenerator : IMovementDocumentGenerator
    {
        public byte[] Generate(Movement movement, NotificationApplication notification)
        {
            using (var memoryStream = DocumentHelper.ReadDocumentStreamShared("MovementMergeTemplate.docx"))
            {
                using (var document = WordprocessingDocument.Open(memoryStream, true))
                {
                    var movementDocument = new MovementDocument(document, movement, notification);

                    movementDocument.Merge();

                    MergeFieldLocator.RemoveDataSourceSettingFromMergedDocument(document);
                }

                return memoryStream.ToArray();
            }
        }
    }
}
