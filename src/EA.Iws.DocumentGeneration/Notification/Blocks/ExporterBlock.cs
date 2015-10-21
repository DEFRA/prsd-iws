namespace EA.Iws.DocumentGeneration.Notification.Blocks
{
    using System.Collections.Generic;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Exporter;
    using Mapper;
    using ViewModels;

    internal class ExporterBlock : IDocumentBlock
    {
        private readonly ExporterViewModel data;

        public ExporterBlock(IList<MergeField> mergeFields, Exporter exporter)
        {
            CorrespondingMergeFields = MergeFieldLocator.GetCorrespondingFieldsForBlock(mergeFields, TypeName);

            data = new ExporterViewModel(exporter);
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