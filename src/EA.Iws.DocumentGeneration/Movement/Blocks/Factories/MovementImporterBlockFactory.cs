namespace EA.Iws.DocumentGeneration.Movement.Blocks.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.NotificationApplication.Importer;

    internal class MovementImporterBlockFactory : IMovementBlockFactory
    {
        private readonly IImporterRepository importerRepository;

        public MovementImporterBlockFactory(IImporterRepository importerRepository)
        {
            this.importerRepository = importerRepository;
        }

        public async Task<IDocumentBlock> Create(Guid movementId, IList<MergeField> mergeFields)
        {
            var importer = await importerRepository.GetByMovementId(movementId);
            return new MovementImporterBlock(mergeFields, importer);
        }
    }
}