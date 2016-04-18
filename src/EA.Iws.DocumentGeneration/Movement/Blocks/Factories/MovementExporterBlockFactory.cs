namespace EA.Iws.DocumentGeneration.Movement.Blocks.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.NotificationApplication.Exporter;

    internal class MovementExporterBlockFactory : IMovementBlockFactory
    {
        private readonly IExporterRepository exporterRepository;

        public MovementExporterBlockFactory(IExporterRepository exporterRepository)
        {
            this.exporterRepository = exporterRepository;
        }

        public async Task<IDocumentBlock> Create(Guid movementId, IList<MergeField> mergeFields)
        {
            var exporter = await exporterRepository.GetByMovementId(movementId);
            return new MovementExporterBlock(mergeFields, exporter);
        }
    }
}