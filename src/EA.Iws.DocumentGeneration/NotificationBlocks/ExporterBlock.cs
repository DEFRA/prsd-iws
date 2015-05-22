namespace EA.Iws.DocumentGeneration.NotificationBlocks
{
    using System.Collections.Generic;
    using Domain.Notification;
    using Mapper;
    using ViewModels;

    internal class ExporterBlock : INotificationBlock
    {
        private readonly ExporterViewModel data;

        public ExporterBlock(IList<MergeField> mergeFields, NotificationApplication notification)
        {
            CorrespondingMergeFields = MergeFieldLocator.GetCorrespondingFieldsForBlock(mergeFields, "Exporter");

            data = new ExporterViewModel(notification.Exporter);
        }

        public string TypeName
        {
            get { return "Exporter"; }
        }

        public ICollection<MergeField> CorrespondingMergeFields { get; private set; }

        public void Merge()
        {
            var properties = PropertyHelper.GetPropertiesForViewModel(typeof(ExporterViewModel));

            foreach (var field in CorrespondingMergeFields)
            {
                MergeFieldDataMapper.BindCorrespondingField(field, data, properties);
            }
        }

        public int OrdinalPosition
        {
            get { return 1; }
        }
    }
}