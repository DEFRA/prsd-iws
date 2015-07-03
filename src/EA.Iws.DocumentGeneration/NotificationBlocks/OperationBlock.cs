namespace EA.Iws.DocumentGeneration.NotificationBlocks
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Notification;
    using Mapper;
    using ViewModels;

    internal class OperationBlock : INotificationBlock, IAnnexedBlock
    {
        private readonly OperationViewModel data;

        public OperationBlock(IList<MergeField> mergeFields, NotificationApplication notification)
        {
            CorrespondingMergeFields = MergeFieldLocator.GetCorrespondingFieldsForBlock(mergeFields, TypeName);
            data = new OperationViewModel(notification);

            AnnexMergeFields = MergeFieldLocator.GetAnnexMergeFields(mergeFields, TypeName);
            ((List<MergeField>)AnnexMergeFields).AddRange(MergeFieldLocator.GetAnnexMergeFields(mergeFields, TypeName));
        }

        public ICollection<MergeField> CorrespondingMergeFields { get; private set; }

        public string TypeName
        {
            get { return "Operation"; }
        }

        public int OrdinalPosition
        {
            get { return 11; }
        }

        public void Merge()
        {
            if (!HasAnnex)
            {
                var properties = PropertyHelper.GetPropertiesForViewModel(typeof(OperationViewModel));
                foreach (var field in CorrespondingMergeFields)
                {
                    MergeFieldDataMapper.BindCorrespondingField(field, data, properties);
                }

                ClearAnnexFields();
            }
        }

        public IList<MergeField> AnnexMergeFields { get; private set; }

        public bool HasAnnex
        {
            get { return data.TechnologyEmployed.Length > 200; }
        }

        public void GenerateAnnex(int annexNumber)
        {
            MergeOperationToMainDocument(annexNumber);
            MergeAnnexNumber(annexNumber);
        }

        private void MergeOperationToMainDocument(int annexNumber)
        {
            var properties = PropertyHelper.GetPropertiesForViewModel(typeof(OperationViewModel));
            foreach (var field in CorrespondingMergeFields)
            {
                MergeFieldDataMapper.BindCorrespondingField(field, new OperationViewModel(data, annexNumber), properties);
            }

            foreach (var annexMergeField in AnnexMergeFields)
            {
                MergeFieldDataMapper.BindCorrespondingField(annexMergeField, data, properties);
            }
        }

        private void ClearAnnexFields()
        {
            foreach (var annexMergeField in AnnexMergeFields)
            {
                annexMergeField.RemoveCurrentContents();
            }
        }

        private void MergeAnnexNumber(int annexNumber)
        {
            // Set the annex number as the page title.
            var annexNumberField = FindAnnexNumberMergeField();
            annexNumberField.RemoveCurrentContents();
            annexNumberField.SetText(annexNumber.ToString(), 0);
        }

        private MergeField FindAnnexNumberMergeField()
        {
            return AnnexMergeFields.FirstOrDefault(mf => mf.FieldName.InnerTypeName != null
                    && mf.FieldName.OuterTypeName.Equals("Operation")
                    && mf.FieldName.InnerTypeName.Equals("AnnexNumber"));
        }
    }
}