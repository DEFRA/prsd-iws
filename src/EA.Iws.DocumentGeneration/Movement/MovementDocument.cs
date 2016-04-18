namespace EA.Iws.DocumentGeneration.Movement
{
    using System.Collections.Generic;
    using System.Linq;
    using Notification.Blocks;

    public class MovementDocument
    {
        private readonly IEnumerable<IDocumentBlock> movementBlocks;

        public MovementDocument(IEnumerable<IDocumentBlock> movementBlocks)
        {
            this.movementBlocks = movementBlocks;
        }

        public void Merge(bool hasCarrierAnnex)
        {
            int i = (hasCarrierAnnex) ? 2 : 1;
            foreach (var block in movementBlocks.OrderBy(b => b.OrdinalPosition))
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
    }
}
