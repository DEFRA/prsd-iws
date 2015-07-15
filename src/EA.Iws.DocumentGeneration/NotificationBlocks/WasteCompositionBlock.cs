namespace EA.Iws.DocumentGeneration.NotificationBlocks
{
    using System.Collections.Generic;
    using Domain.Notification;
    using Mapper;
    using ViewModels;

    internal class WasteCompositionBlock : INotificationBlock
    {
        private readonly WasteCompositionViewModel data;

        public WasteCompositionBlock(IList<MergeField> mergeFields, NotificationApplication notification)
        {
            CorrespondingMergeFields = MergeFieldLocator.GetCorrespondingFieldsForBlock(mergeFields, TypeName);
            data = new WasteCompositionViewModel(notification.WasteType);
        }

        public ICollection<MergeField> CorrespondingMergeFields { get; private set; }

        public string TypeName
        {
            get { return "WasteComposition"; }
        }

        public int OrdinalPosition
        {
            get { return 12; }
        }

        public void Merge()
        {
            var properties = PropertyHelper.GetPropertiesForViewModel(typeof(WasteCompositionViewModel));

            foreach (var field in CorrespondingMergeFields)
            {
                MergeFieldDataMapper.BindCorrespondingField(field, data, properties);
            }
        }
    }
}