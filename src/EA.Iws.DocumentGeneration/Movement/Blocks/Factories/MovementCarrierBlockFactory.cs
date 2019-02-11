namespace EA.Iws.DocumentGeneration.Movement.Blocks.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Movement;

    internal class MovementCarrierBlockFactory : IMovementBlockFactory
    {
        private readonly IMovementCarrierRepository repository;

        public MovementCarrierBlockFactory(IMovementCarrierRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IDocumentBlock> Create(Guid movementId, IList<MergeField> mergeFields)
        {
            var movementCarriers = await repository.GetCarriersByMovementId(movementId);

            return new MovementCarrierBlock(mergeFields, movementCarriers);
        }
    }
}