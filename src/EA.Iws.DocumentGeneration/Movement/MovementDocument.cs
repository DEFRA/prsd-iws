namespace EA.Iws.DocumentGeneration.Movement
{
    using System.Linq;
    using DocumentFormat.OpenXml.Packaging;
    using Domain.Movement;
    using Formatters;
    using MovementBlocks;
    using NotificationBlocks;

    public class MovementDocument
    {
        private readonly WordprocessingDocument document;
        private readonly MovementBlockCollection movementBlockCollection;
        private readonly bool hasCarrierAnnex;

        public MovementDocument(WordprocessingDocument document, Movement movement)
        {
            this.document = document;
            var fields = MergeFieldLocator.GetMergeRuns(document);

            if (movement != null && movement.NotificationApplication != null)
            {
                hasCarrierAnnex = movement.NotificationApplication.Carriers.Count() > 1;
            }

            movementBlockCollection = new MovementBlockCollection(fields, movement);
            ApplyUnitStrikethrough(movement);
        }

        public void Merge()
        {
            int i = (hasCarrierAnnex) ? 2 : 1;
            foreach (var block in movementBlockCollection.OrderBy(b => b.OrdinalPosition))
            {
                block.Merge();

                var annexBlock = block as IAnnexedBlock;

                if (annexBlock != null && annexBlock.HasAnnex)
                {
                    annexBlock.GenerateAnnex(i);
                    i++;
                }
            }
        }

        private void ApplyUnitStrikethrough(Movement movement)
        {
            if (movement != null && movement.Units.HasValue)
            {
                ShipmentQuantityUnitFormatter.ApplyStrikethroughFormattingToUnits(document, 
                    movement.Units.Value);
            }
        }
    }
}
