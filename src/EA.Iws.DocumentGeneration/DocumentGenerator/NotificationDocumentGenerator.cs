namespace EA.Iws.DocumentGeneration.DocumentGenerator
{
    using System;
    using System.Threading.Tasks;
    using DocumentFormat.OpenXml.Packaging;
    using Domain;
    using Domain.NotificationApplication.Shipment;
    using Formatters;
    using Notification;

    public class NotificationDocumentGenerator : INotificationDocumentGenerator
    {
        private readonly NotificationBlocksFactory blocksFactory;
        private readonly IShipmentInfoRepository shipmentInfoRepository;

        public NotificationDocumentGenerator(IShipmentInfoRepository shipmentInfoRepository,
            NotificationBlocksFactory blocksFactory)
        {
            this.shipmentInfoRepository = shipmentInfoRepository;
            this.blocksFactory = blocksFactory;
        }

        public async Task<byte[]> GenerateNotificationDocument(Guid notificationId)
        {
            using (var memoryStream = DocumentHelper.ReadDocumentStreamShared("NotificationMergeTemplate.docx"))
            {
                using (var document = WordprocessingDocument.Open(memoryStream, true))
                {
                    var mergeFields = MergeFieldLocator.GetMergeRuns(document);

                    var blocks = await blocksFactory.GetBlocks(notificationId, mergeFields);

                    var notificationDocument = new NotificationDocumentMerger(mergeFields, blocks);

                    var shipmentInfo = await shipmentInfoRepository.GetByNotificationId(notificationId);
                    ShipmentQuantityUnitFormatter.ApplyStrikethroughFormattingToUnits(document, shipmentInfo);

                    notificationDocument.Merge();

                    MergeFieldLocator.RemoveDataSourceSettingFromMergedDocument(document);
                }

                return memoryStream.ToArray();
            }
        }
    }
}