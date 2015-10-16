namespace EA.Iws.DocumentGeneration.Movement.Blocks.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Movement;

    internal class MovementCarrierBlockFactory : IMovementBlockFactory
    {
        private readonly IMovementRepository movementRepository;

        public MovementCarrierBlockFactory(IMovementRepository movementRepository)
        {
            this.movementRepository = movementRepository;
        }

        public async Task<IDocumentBlock> Create(Guid movementId, IList<MergeField> mergeFields)
        {
            var movement = await movementRepository.GetById(movementId);
            return new MovementCarrierBlock(mergeFields, movement);
        }
    }
}