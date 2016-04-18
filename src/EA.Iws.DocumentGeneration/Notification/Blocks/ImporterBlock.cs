namespace EA.Iws.DocumentGeneration.Notification.Blocks
{
    using System.Collections.Generic;
    using Domain.NotificationApplication.Importer;
    using Mapper;
    using ViewModels;

    internal class ImporterBlock : IDocumentBlock
    {
        private readonly ImporterViewModel data;

        public ImporterBlock(IList<MergeField> mergeFields, Importer importer)
        {
            CorrespondingMergeFields = MergeFieldLocator.GetCorrespondingFieldsForBlock(mergeFields, "Importer");

            data = new ImporterViewModel(importer);
        }

        public string TypeName
        {
            get { return "Importer"; }
        }

        public ICollection<MergeField> CorrespondingMergeFields { get; private set; }

        public void Merge()
        {
            var properties = PropertyHelper.GetPropertiesForViewModel(typeof(ImporterViewModel));

            foreach (var field in CorrespondingMergeFields)
            {
                MergeFieldDataMapper.BindCorrespondingField(field, data, properties);
            }
        }

        public int OrdinalPosition
        {
            get { return 2; }
        }
    }
}