namespace EA.Iws.DocumentGeneration.Movement.Blocks.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Movement;

    internal class MovementCarrierBlockFactory : IMovementBlockFactory
    {
        private readonly IMovementDetailsRepository repository;

        public MovementCarrierBlockFactory(IMovementDetailsRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IDocumentBlock> Create(Guid movementId, IList<MergeField> mergeFields)
        {
            var movementDetails = await repository.GetByMovementId(movementId);
            return new MovementCarrierBlock(mergeFields, movementDetails);
        }
    }
}