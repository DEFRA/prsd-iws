namespace EA.Iws.DocumentGeneration.DocumentGenerator
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using DocumentFormat.OpenXml.Packaging;
    using Domain;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using Formatters;
    using Movement;

    public class MovementDocumentGenerator : IMovementDocumentGenerator
    {
        private readonly IMovementRepository movementRepository;
        private readonly INotificationApplicationRepository notificationApplicationRepository;
        private readonly MovementBlocksFactory blocksFactory;

        public MovementDocumentGenerator(IMovementRepository movementRepository, INotificationApplicationRepository notificationApplicationRepository, MovementBlocksFactory blocksFactory)
        {
            this.movementRepository = movementRepository;
            this.notificationApplicationRepository = notificationApplicationRepository;
            this.blocksFactory = blocksFactory;
        }

        public async Task<byte[]> Generate(Guid movementId)
        {
            using (var memoryStream = DocumentHelper.ReadDocumentStreamShared("MovementMergeTemplate.docx"))
            {
                using (var document = WordprocessingDocument.Open(memoryStream, true))
                {
                    var movement = await movementRepository.GetById(movementId);
                    var notification = await notificationApplicationRepository.GetByMovementId(movementId);
                    bool hasCarrierAnnex = notification.Carriers.Count() > 1;

                    var fields = MergeFieldLocator.GetMergeRuns(document);
                    var blocks = await blocksFactory.GetBlocks(movementId, fields);

                    var movementDocument = new MovementDocument(blocks);

                    ShipmentQuantityUnitFormatter.ApplyStrikethroughFormattingToUnits(document, movement.Units.Value);

                    movementDocument.Merge(hasCarrierAnnex);

                    MergeFieldLocator.RemoveDataSourceSettingFromMergedDocument(document);
                }

                return memoryStream.ToArray();
            }
        }
    }
}
