namespace EA.Iws.DocumentGeneration.DocumentGenerator
{
    using DocumentFormat.OpenXml.Packaging;
    using Domain;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Shipment;
    using Movement;

    public class MovementDocumentGenerator : IMovementDocumentGenerator
    {
        public byte[] Generate(Movement movement, NotificationApplication notification, ShipmentInfo shipmentInfo)
        {
            using (var memoryStream = DocumentHelper.ReadDocumentStreamShared("MovementMergeTemplate.docx"))
            {
                using (var document = WordprocessingDocument.Open(memoryStream, true))
                {
                    var movementDocument = new MovementDocument(document, movement, notification, shipmentInfo);

                    movementDocument.Merge();

                    MergeFieldLocator.RemoveDataSourceSettingFromMergedDocument(document);
                }

                return memoryStream.ToArray();
            }
        }
    }
}
