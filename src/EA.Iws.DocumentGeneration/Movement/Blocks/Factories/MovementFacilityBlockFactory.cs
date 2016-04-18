namespace EA.Iws.DocumentGeneration.Movement.Blocks.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.NotificationApplication;

    internal class MovementFacilityBlockFactory : IMovementBlockFactory
    {
        private readonly IFacilityRepository facilityRepository;

        public MovementFacilityBlockFactory(IFacilityRepository facilityRepository)
        {
            this.facilityRepository = facilityRepository;
        }

        public async Task<IDocumentBlock> Create(Guid movementId, IList<MergeField> mergeFields)
        {
            var facilityCollection = await facilityRepository.GetByMovementId(movementId);
            return new MovementFacilityBlock(mergeFields, facilityCollection);
        }
    }
}