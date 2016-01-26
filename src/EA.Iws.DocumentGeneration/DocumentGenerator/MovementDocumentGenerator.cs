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
        private readonly ICarrierRepository carrierRepository;
        private readonly IMovementDetailsRepository movementDetailsRepository;
        private readonly MovementBlocksFactory blocksFactory;

        public MovementDocumentGenerator(ICarrierRepository carrierRepository,
            IMovementDetailsRepository movementDetailsRepository,
            MovementBlocksFactory blocksFactory)
        {
            this.blocksFactory = blocksFactory;
            this.movementDetailsRepository = movementDetailsRepository;
            this.carrierRepository = carrierRepository;
        }

        public async Task<byte[]> Generate(Guid movementId)
        {
            using (var memoryStream = DocumentHelper.ReadDocumentStreamShared("MovementMergeTemplate.docx"))
            {
                using (var document = WordprocessingDocument.Open(memoryStream, true))
                {
                    var movementDetails = await movementDetailsRepository.GetByMovementId(movementId);
                    var carrierCollection = await carrierRepository.GetByMovementId(movementId);
                    bool hasCarrierAnnex = carrierCollection.Carriers.Count() > 1;

                    var fields = MergeFieldLocator.GetMergeRuns(document);
                    var blocks = await blocksFactory.GetBlocks(movementId, fields);

                    var movementDocument = new MovementDocument(blocks);

                    ShipmentQuantityUnitFormatter.ApplyStrikethroughFormattingToUnits(document, movementDetails.ActualQuantity.Units);

                    movementDocument.Merge(hasCarrierAnnex);

                    MergeFieldLocator.RemoveDataSourceSettingFromMergedDocument(document);
                }

                return memoryStream.ToArray();
            }
        }
    }
}
