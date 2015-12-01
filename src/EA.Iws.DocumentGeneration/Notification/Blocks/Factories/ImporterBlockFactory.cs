namespace EA.Iws.DocumentGeneration.Notification.Blocks.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.NotificationApplication.Importer;

    internal class ImporterBlockFactory : INotificationBlockFactory
    {
        private readonly IImporterRepository importerRepository;

        public ImporterBlockFactory(IImporterRepository importerRepository)
        {
            this.importerRepository = importerRepository;
        }

        public async Task<IDocumentBlock> Create(Guid notificationId, IList<MergeField> mergeFields)
        {
            var importer = await importerRepository.GetByNotificationId(notificationId);
            return new ImporterBlock(mergeFields, importer);
        }
    }
}