namespace EA.Iws.DocumentGeneration.NotificationBlocks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Domain.NotificationApplication;
    using Mapper;
    using ViewModels;

    internal class SpecialHandlingBlock : INotificationBlock, IAnnexedBlock
    {
        private const string SpecialHandling = "SpHandling";
        private readonly SpecialHandlingViewModel data;

        public SpecialHandlingBlock(IList<MergeField> mergeFields, NotificationApplication notification)
        {
            data = new SpecialHandlingViewModel(notification);
            CorrespondingMergeFields = MergeFieldLocator.GetCorrespondingFieldsForBlock(mergeFields, TypeName);
            AnnexMergeFields = MergeFieldLocator.GetAnnexMergeFields(mergeFields, SpecialHandling);
        }

        public IList<MergeField> AnnexMergeFields { get; private set; }

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

            ClearAnnexFields();
        }

        private void ClearAnnexFields()
        {
            foreach (var field in FindAllAnnexMergeFields())
            {
                field.RemoveCurrentContents();
            }
        }

        private void MergeAnnexNumber(int annexNumber)
        {
            var annexNumberField = FindAnnexNumberMergeField();
            annexNumberField.RemoveCurrentContents();
            annexNumberField.SetText(annexNumber.ToString(), 0);
        }

        private MergeField FindAnnexNumberMergeField()
        {
            return AnnexMergeFields.Single(mf => mf.FieldName.InnerTypeName != null
                                    && mf.FieldName.InnerTypeName.Equals("AnnexNumber"));
        }

        private IEnumerable<MergeField> FindAllAnnexMergeFields()
        {
            return AnnexMergeFields.Where(mf => !string.IsNullOrWhiteSpace(mf.FieldName.OuterTypeName)
                                     && mf.FieldName.OuterTypeName.Equals(SpecialHandling));
        }
    }
}
