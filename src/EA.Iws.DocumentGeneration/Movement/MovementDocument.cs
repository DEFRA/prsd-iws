namespace EA.Iws.DocumentGeneration.Movement
{
    using System.Linq;
    using DocumentFormat.OpenXml.Packaging;
    using Domain.Movement;
    using Formatters;
    using MovementBlocks;

    public class MovementDocument
    {
        private readonly WordprocessingDocument document;
        private readonly MovementBlockCollection movementBlockCollection;

        public MovementDocument(WordprocessingDocument document, Movement movement)
        {
            this.document = document;
            var fields = MergeFieldLocator.GetMergeRuns(document);
            movementBlockCollection = new MovementBlockCollection(fields, movement);
            ApplyUnitStrikethrough(movement);
        }

        public void Merge()
        {
            foreach (var block in movementBlockCollection.OrderBy(b => b.OrdinalPosition))
            {
                block.Merge();
            }

            //TODO: Generate carrier annex
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
