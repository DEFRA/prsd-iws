namespace EA.Iws.DocumentGeneration.Notification.Blocks
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Domain.NotificationApplication;
    using Mapper;
    using ViewModels;

    internal class SpecialHandlingBlock : AnnexBlockBase, IDocumentBlock, IAnnexedBlock
    {
        private const string SpecialHandling = "SpHandling";
        private readonly SpecialHandlingViewModel data;

        public SpecialHandlingViewModel Data
        {
            get { return data; }
        }

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

        public virtual void Merge()
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

        public virtual void GenerateAnnex(int annexNumber)
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

        protected void MergeSpecialHandlingDataToDocument(SpecialHandlingViewModel specialHandling, PropertyInfo[] properties)
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
