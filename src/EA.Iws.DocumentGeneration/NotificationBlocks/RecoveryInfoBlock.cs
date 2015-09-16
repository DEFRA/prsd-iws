namespace EA.Iws.DocumentGeneration.NotificationBlocks
{
    using System.Collections.Generic;
    using Domain.NotificationApplication;
    using Formatters;
    using Mapper;
    using ViewModels;

    internal class RecoveryInfoBlock : AnnexBlockBase, INotificationBlock, IAnnexedBlock
    {
        private const string RecoveryInfo = "RecovInfo";
        private readonly RecoveryInfoViewModel data;

        public RecoveryInfoBlock(IList<MergeField> mergeFields, NotificationApplication notification)
        {
            AnnexMergeFields = MergeFieldLocator.GetAnnexMergeFields(mergeFields, TypeName);

            CorrespondingMergeFields = MergeFieldLocator.GetCorrespondingFieldsForBlock(mergeFields, TypeName);
            data = new RecoveryInfoViewModel(notification, new RecoveryInfoFormatter());

            if (notification.NotificationType == NotificationType.Disposal)
            {
                HasAnnex = false;

                MergeMainDocumentBlock();
            }
            else
            {
                HasAnnex = true;

                if (notification.IsProvidedByImporter.GetValueOrDefault())
                {
                    HasAnnex = false;

                    MergeMainDocumentBlock();
                }
            }
        }

        private void MergeMainDocumentBlock()
        {
            var properties = PropertyHelper.GetPropertiesForViewModel(typeof(RecoveryInfoViewModel));
            foreach (var field in CorrespondingMergeFields)
            {
                MergeFieldDataMapper.BindCorrespondingField(field, data, properties);
            }
        }

        public string TypeName
        {
            get { return RecoveryInfo; }
        }

        public ICollection<MergeField> CorrespondingMergeFields { get; private set; }

        public void Merge()
        {
            if (!HasAnnex)
            {
                RemoveAnnex();
            }
        }

        public int OrdinalPosition
        {
            get { return 11; }
        }

        public bool HasAnnex { get; private set; }

        public void GenerateAnnex(int annexNumber)
        {
            var properties = PropertyHelper.GetPropertiesForViewModel(typeof(RecoveryInfoViewModel));
            foreach (var field in CorrespondingMergeFields)
            {
                MergeFieldDataMapper.BindCorrespondingField(field, new RecoveryInfoViewModel(data, annexNumber), properties);
            }

            foreach (var annexMergeField in AnnexMergeFields)
            {
                MergeFieldDataMapper.BindCorrespondingField(annexMergeField, data, properties);
            }

            MergeAnnexNumber(annexNumber);

            TocText = "Annex " + annexNumber + " - Genuine recovery information";
        }
    }
}
