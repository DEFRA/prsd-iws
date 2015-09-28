namespace EA.Iws.DocumentGeneration.DocumentGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DataAccess;
    using DocumentFormat.OpenXml.Packaging;
    using Domain;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Recovery;
    using Domain.NotificationApplication.Shipment;
    using Domain.TransportRoute;
    using Formatters;
    using NotificationBlocks;
    using System.Threading.Tasks;

    public class NotificationDocumentGenerator : INotificationDocumentGenerator
    {
        private string TocText { get; set; }
        private string InstructionsText { get; set; }
        private readonly INotificationApplicationRepository notificationRepository;
        private readonly IShipmentInfoRepository shipmentInfoRepository;
        private readonly ITransportRouteRepository transportRouteRepository;
        private readonly IRecoveryInfoRepository recoveryInfoRepository;

        public NotificationDocumentGenerator(INotificationApplicationRepository notificationRepository,
            IShipmentInfoRepository shipmentInfoRepository,
            ITransportRouteRepository transportRouteRepository,
            IRecoveryInfoRepository recoveryInfoRepository)
        {
            this.notificationRepository = notificationRepository;
            this.shipmentInfoRepository = shipmentInfoRepository;
            this.transportRouteRepository = transportRouteRepository;
            this.recoveryInfoRepository = recoveryInfoRepository;
        }

        public async Task<byte[]> GenerateNotificationDocument(Guid notificationId)
        {
            using (var memoryStream = DocumentHelper.ReadDocumentStreamShared("NotificationMergeTemplate.docx"))
            {
                using (var document = WordprocessingDocument.Open(memoryStream, true))
                {
                    var notification = await notificationRepository.GetById(notificationId);
                    var shipmentInfo = await shipmentInfoRepository.GetByNotificationId(notificationId);
                    var transportRoute = await transportRouteRepository.GetByNotificationId(notificationId);
                    var recoveryInfo = await recoveryInfoRepository.GetByNotificationId(notificationId);

                    ShipmentQuantityUnitFormatter.ApplyStrikethroughFormattingToUnits(document, shipmentInfo);

                    // Get all merge fields.
                    var mergeFields = MergeFieldLocator.GetMergeRuns(document);

                    var blocks = GetBlocks(notification, shipmentInfo, transportRoute, recoveryInfo, mergeFields);

                    foreach (var block in blocks)
                    {
                        block.Merge();
                    }

                    int annexNumber = 1;
                    foreach (var block in blocks.OrderBy(b => b.OrdinalPosition))
                    {
                        var annexBlock = block as IAnnexedBlock;
                        if (annexBlock != null && annexBlock.HasAnnex)
                        {
                            annexBlock.GenerateAnnex(annexNumber);

                            var newTocText = string.IsNullOrEmpty(annexBlock.TocText) ? string.Empty : annexBlock.TocText + Environment.NewLine;
                            TocText = TocText + newTocText;

                            var newInstructionsText = string.IsNullOrEmpty(annexBlock.InstructionsText) ? string.Empty : annexBlock.InstructionsText + Environment.NewLine;
                            InstructionsText = InstructionsText + newInstructionsText;

                            annexNumber++;
                        }
                    }

                    var finalBlock = new NumberOfAnnexesAndInstructionsAndToCBlock(mergeFields, annexNumber - 1, TocText, InstructionsText);
                    finalBlock.Merge();

                    MergeFieldLocator.RemoveDataSourceSettingFromMergedDocument(document);
                }

                return memoryStream.ToArray();
            }
        }

        private static IList<IDocumentBlock> GetBlocks(NotificationApplication notification, ShipmentInfo shipmentInfo, TransportRoute transportRoute, RecoveryInfo recoveryInfo, IList<MergeField> mergeFields)
        {
            return new List<IDocumentBlock>
            {
                new GeneralBlock(mergeFields, notification, shipmentInfo),
                new ExporterBlock(mergeFields, notification),
                new ProducerBlock(mergeFields, notification),
                new ImporterBlock(mergeFields, notification),
                new FacilityBlock(mergeFields, notification),
                new OperationBlock(mergeFields, notification),
                new RecoveryInfoBlock(mergeFields, notification, recoveryInfo),
                new CarrierBlock(mergeFields, notification),
                new SpecialHandlingBlock(mergeFields, notification),
                new WasteCompositionBlock(mergeFields, notification),
                new TransportBlock(mergeFields, transportRoute),
                new WasteCodesBlock(mergeFields, notification),
                new CustomsOfficeBlock(mergeFields, transportRoute),
                new TransitStatesBlock(mergeFields, transportRoute)
            };
        }
    }
}