namespace EA.Iws.DocumentGeneration.NotificationBlocks
{
    using System.Collections.Generic;
    using Domain.NotificationApplication;
    using Mapper;
    using ViewModels;

    internal class GeneralBlock : INotificationBlock
    {
        private readonly GeneralViewModel data;

        public GeneralBlock(IList<MergeField> mergeFields, NotificationApplication notification)
        {
            CorrespondingMergeFields = MergeFieldLocator.GetCorrespondingFieldsForBlock(mergeFields, "General");

            data = new GeneralViewModel(notification);
        }

        public string TypeName
        {
            get { return "General"; }
        }

        public ICollection<MergeField> CorrespondingMergeFields { get; private set; }

        public void Merge()
        {
            var properties = PropertyHelper.GetPropertiesForViewModel(typeof(GeneralViewModel));

            foreach (var field in CorrespondingMergeFields)
            {
                MergeFieldDataMapper.BindCorrespondingField(field, data, properties);
            }
        }

        public int OrdinalPosition
        {
            get { return 3; }
        }
    }
}