namespace EA.Iws.DocumentGeneration.NotificationBlocks
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.NotificationApplication;
    using Mapper;
    using ViewModels;

    internal class CustomsOfficeBlock : INotificationBlock, IAnnexedBlock
    {
        private readonly CustomsOfficeViewModel data;

        public ICollection<MergeField> CorrespondingMergeFields { get; private set; }
        public IList<MergeField> AnnexMergeFields { get; private set; }

        public CustomsOfficeBlock(IList<MergeField> mergeFields, NotificationApplication notification)
        {
            CorrespondingMergeFields = MergeFieldLocator.GetCorrespondingFieldsForBlock(mergeFields, TypeName);
            data = new CustomsOfficeViewModel(notification);

            AnnexMergeFields = MergeFieldLocator.GetAnnexMergeFields(mergeFields, TypeName);
        }

        public string TypeName
        {
            get { return "CustomsOffice"; }
        }

        public int OrdinalPosition
        {
            get { return 16; }
        }

        public bool HasAnnex
        {
            get { return data.IsAnnexNeeded; }
        }

        public void Merge()
        {
            if (!HasAnnex)
            {
                var properties = PropertyHelper.GetPropertiesForViewModel(typeof(CustomsOfficeViewModel));

                foreach (var field in CorrespondingMergeFields)
                {
                    MergeFieldDataMapper.BindCorrespondingField(field, data, properties);
                }

                ClearAnnexFields();
            }
        }

        public void GenerateAnnex(int annexNumber)
        {
            MergeToMainDocument(annexNumber);

            var properties = PropertyHelper.GetPropertiesForViewModel(typeof(CustomsOfficeViewModel));

            foreach (var field in AnnexMergeFields)
            {
                MergeFieldDataMapper.BindCorrespondingField(field, data, properties);
            }

            MergeAnnexNumber(annexNumber);
        }

        private void ClearAnnexFields()
        {
            foreach (var annexMergeField in AnnexMergeFields)
            {
                annexMergeField.RemoveCurrentContents();
            }
        }

        private void MergeToMainDocument(int annexNumber)
        {
            data.SetAnnexMessages(annexNumber);

            var properties = PropertyHelper.GetPropertiesForViewModel(typeof(CustomsOfficeViewModel));
            foreach (var field in CorrespondingMergeFields)
            {
                MergeFieldDataMapper.BindCorrespondingField(field, data, properties);
            }
        }

        private void MergeAnnexNumber(int annexNumber)
        {
            // Set the annex number as the page title.
            foreach (var annexNumberField in FindAnnexNumberMergeFields())
            {
                annexNumberField.RemoveCurrentContents();
                annexNumberField.SetText(annexNumber.ToString(), 0);
            }
        }

        private IEnumerable<MergeField> FindAnnexNumberMergeFields()
        {
            return AnnexMergeFields.Where(mf => mf.FieldName.InnerTypeName != null
                    && mf.FieldName.OuterTypeName.Equals(TypeName)
                    && mf.FieldName.InnerTypeName.Equals("AnnexNumber"));
        }
    }
}
