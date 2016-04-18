namespace EA.Iws.DocumentGeneration.Notification.Blocks
{
    using System.Collections.Generic;
    using Core.Shared;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.WasteRecovery;
    using Formatters;
    using Mapper;
    using ViewModels;

    internal class WasteRecoveryBlock : AnnexBlockBase, IDocumentBlock, IAnnexedBlock
    {
        private const string RecoveryInfo = "RecovInfo";
        private readonly WasteRecoveryViewModel data;

        public WasteRecoveryBlock(IList<MergeField> mergeFields, NotificationApplication notification, WasteRecovery wasteRecovery, WasteDisposal wasteDisposal)
        {
            AnnexMergeFields = MergeFieldLocator.GetAnnexMergeFields(mergeFields, TypeName);

            CorrespondingMergeFields = MergeFieldLocator.GetCorrespondingFieldsForBlock(mergeFields, TypeName);
            data = new WasteRecoveryViewModel(notification, wasteRecovery, wasteDisposal, new WasteRecoveryFormatter());
            
            if (notification.NotificationType == NotificationType.Disposal)
            {
                HasAnnex = false;

                MergeMainDocumentBlock();
            }
            else
            {
                HasAnnex = true;

                if (notification.WasteRecoveryInformationProvidedByImporter.GetValueOrDefault())
                {
                    HasAnnex = false;

                    MergeMainDocumentBlock();
                }
            }
        }

        private void MergeMainDocumentBlock()
        {
            var properties = PropertyHelper.GetPropertiesForViewModel(typeof(WasteRecoveryViewModel));
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
            var properties = PropertyHelper.GetPropertiesForViewModel(typeof(WasteRecoveryViewModel));
            foreach (var field in CorrespondingMergeFields)
            {
                MergeFieldDataMapper.BindCorrespondingField(field, new WasteRecoveryViewModel(data, annexNumber), properties);
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
