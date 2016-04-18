namespace EA.Iws.DocumentGeneration.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Blocks.Factories;

    public class MovementBlocksFactory
    {
        private readonly IEnumerable<IMovementBlockFactory> movementBlockFactories;

        public MovementBlocksFactory(IEnumerable<IMovementBlockFactory> movementBlockFactories)
        {
            this.movementBlockFactories = movementBlockFactories;
        }

        public async Task<List<IDocumentBlock>> GetBlocks(Guid movementId, IList<MergeField> mergeFields)
        {
            var blocks = new List<IDocumentBlock>();
            foreach (var factory in movementBlockFactories)
            {
                blocks.Add(await factory.Create(movementId, mergeFields));
            }
            return blocks;
        }
    }
}