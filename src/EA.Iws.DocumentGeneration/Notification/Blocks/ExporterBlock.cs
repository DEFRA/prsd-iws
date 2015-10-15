namespace EA.Iws.DocumentGeneration.Notification.Blocks
{
    using System.Collections.Generic;
    using Domain.NotificationApplication;
    using Mapper;
    using ViewModels;

    internal class ExporterBlock : IDocumentBlock
    {
        private readonly ExporterViewModel data;

        public ExporterBlock(IList<MergeField> mergeFields, NotificationApplication notification)
        {
            CorrespondingMergeFields = MergeFieldLocator.GetCorrespondingFieldsForBlock(mergeFields, TypeName);

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