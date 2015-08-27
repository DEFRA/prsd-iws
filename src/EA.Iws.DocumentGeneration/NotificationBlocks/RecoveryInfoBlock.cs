namespace EA.Iws.DocumentGeneration.NotificationBlocks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.NotificationApplication;
    using Mapper;
    using ViewModels;

    internal class RecoveryInfoBlock : AnnexBlockBase, INotificationBlock, IAnnexedBlock
    {
        private const string RecoveryInfo = "RecovInfo";
        private readonly RecoveryInfoViewModel data;

        public RecoveryInfoBlock(IList<MergeField> mergeFields, NotificationApplication notification)
        {
            AnnexMergeFields = MergeFieldLocator.GetAnnexMergeFields(mergeFields, TypeName);
            
            if (notification.NotificationType == NotificationType.Disposal)
            {
                RemoveAnnexMessage(mergeFields);
                HasAnnex = false;
                return;
            }

            HasAnnex = true;
            CorrespondingMergeFields = MergeFieldLocator.GetCorrespondingFieldsForBlock(mergeFields, TypeName);
            data = new RecoveryInfoViewModel(notification.RecoveryInfo, notification);
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
            MergeOperationToMainDocument(annexNumber);

            MergeAnnexNumber(annexNumber);

            TocText = "Annex " + annexNumber + " - Genuine Recovery Information";
        }

        private void MergeOperationToMainDocument(int annexNumber)
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
        }

        private void RemoveAnnexMessage(IList<MergeField> mergeFields)
        {
            var annexMessageMergeField = mergeFields.Single(mf => mf.FieldName.OuterTypeName.Equals(RecoveryInfo, StringComparison.InvariantCultureIgnoreCase)
                          && mf.FieldName.InnerTypeName.Equals("AnnexMessage", StringComparison.InvariantCultureIgnoreCase));

            annexMessageMergeField.RemoveCurrentContents();
        }
    }
}
