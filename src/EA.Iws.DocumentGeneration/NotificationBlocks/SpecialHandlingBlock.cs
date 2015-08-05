namespace EA.Iws.DocumentGeneration.NotificationBlocks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Domain.NotificationApplication;
    using Mapper;
    using ViewModels;

    internal class SpecialHandlingBlock : AnnexBlockBase, INotificationBlock, IAnnexedBlock
    {
        private const string SpecialHandling = "SpHandling";
        private readonly SpecialHandlingViewModel data;

        public SpecialHandlingBlock(IList<MergeField> mergeFields, NotificationApplication notification)
        {
            data = new SpecialHandlingViewModel(notification);
            CorrespondingMergeFields = MergeFieldLocator.GetCorrespondingFieldsForBlock(mergeFields, TypeName);
            AnnexMergeFields = MergeFieldLocator.GetAnnexMergeFields(mergeFields, SpecialHandling);
        }

        public bool HasAnnex
        {
            get { return !String.IsNullOrWhiteSpace(data.Requirements); }
        }

        public string TypeName
        {
            get { return "SpecialHandling"; }
        }

        public ICollection<MergeField> CorrespondingMergeFields { get; private set; }

        public void Merge()
        {
            var properties = PropertyHelper.GetPropertiesForViewModel(typeof(SpecialHandlingViewModel));
            if (HasAnnex)
            {
                MergeSpecialHandlingDataToDocument(data, properties);
            }
            else
            {
                ClearAllFields();
            }
        }

        public int OrdinalPosition
        {
            get { return 7; }
        }

        public void GenerateAnnex(int annexNumber)
        {
            if (!HasAnnex)
            {
                ClearAllFields();
                return;
            }

            var properties = PropertyHelper.GetPropertiesForViewModel(typeof(SpecialHandlingViewModel));
            MergeSpecialHandlingDataToDocument(SpecialHandlingViewModel.GetSpecialHandlingAnnexNotice(data, annexNumber), properties);
            MergeAnnexNumber(annexNumber);

            TocText = "Annex " + annexNumber + " - Special handling requirements";
        }

        private void MergeSpecialHandlingDataToDocument(SpecialHandlingViewModel specialHandling, PropertyInfo[] properties)
        {
            foreach (var field in CorrespondingMergeFields)
            {
                MergeFieldDataMapper.BindCorrespondingField(field, specialHandling, properties);
            }

            foreach (var annexField in AnnexMergeFields)
            {
                MergeFieldDataMapper.BindCorrespondingField(annexField, specialHandling, properties);
            }
        }

        private void ClearAllFields()
        {
            foreach (var field in CorrespondingMergeFields)
            {
                field.RemoveCurrentContents();
            }

            RemoveAnnex();
        }
    }
}
