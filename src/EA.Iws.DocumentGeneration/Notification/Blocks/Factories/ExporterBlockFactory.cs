namespace EA.Iws.DocumentGeneration.Notification.Blocks.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.NotificationApplication.Exporter;

    internal class ExporterBlockFactory : INotificationBlockFactory
    {
        private readonly IExporterRepository exporterRepository;

        public ExporterBlockFactory(IExporterRepository exporterRepository)
        {
            this.exporterRepository = exporterRepository;
        }

        public async Task<IDocumentBlock> Create(Guid notificationId, IList<MergeField> mergeFields)
        {
            var exporter = await exporterRepository.GetByNotificationId(notificationId);
            return new ExporterBlock(mergeFields, exporter);
        }
    }
}