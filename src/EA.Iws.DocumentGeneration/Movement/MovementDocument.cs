namespace EA.Iws.DocumentGeneration.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Movement;

    public class MovementDocument
    {
        private ICollection<IDocumentBlock> movementBlocks = new List<IDocumentBlock>(); 

        public MovementDocument(Movement movement)
        {
            GetBlocks(movement);
        }

        public void Merge()
        {
            foreach (var block in movementBlocks.OrderBy(b => b.OrdinalPosition))
            {
                block.Merge();
            }

            //TODO: Generate carrier annex
            throw new NotImplementedException();
        }

        private void GetBlocks(Movement movement)
        {
            //TODO: Add correct blocks
            throw new NotImplementedException();
        }
    }
}
