namespace EA.Iws.DocumentGeneration.Notification.Blocks
{
    using System.Collections.Generic;
    using Domain.NotificationApplication;
    using Formatters;
    using Mapper;
    using ViewModels;

    internal class OperationBlock : AnnexBlockBase, IDocumentBlock, IAnnexedBlock
    {
        private readonly OperationViewModel data;

        public OperationViewModel Data 
        {
            get { return data; }
        }

        public OperationBlock(IList<MergeField> mergeFields, NotificationApplication notification, TechnologyEmployed technologyEmployed)
        {
            CorrespondingMergeFields = MergeFieldLocator.GetCorrespondingFieldsForBlock(mergeFields, TypeName);
            data = new OperationViewModel(notification, technologyEmployed, new OperationInfoFormatter());

            AnnexMergeFields = MergeFieldLocator.GetAnnexMergeFields(mergeFields, TypeName);
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

        public virtual void Merge()
        {
            if (!HasAnnex)
            {
                var properties = PropertyHelper.GetPropertiesForViewModel(typeof(OperationViewModel));
                foreach (var field in CorrespondingMergeFields)
                {
                    MergeFieldDataMapper.BindCorrespondingField(field, data, properties);
                }

                RemoveAnnex();
            }
        }

        public bool HasAnnex
        {
            get { return (data.IsAnnexProvided || !string.IsNullOrEmpty(data.FurtherDetails)); }
        }

        public virtual void GenerateAnnex(int annexNumber)
        {
            MergeOperationToMainDocument(annexNumber);
            MergeAnnexNumber(annexNumber);

            TocText = "Annex " + annexNumber + " - Technology employed";

            if (data.IsAnnexProvided)
            {
                InstructionsText = "Technology employed - annex " + annexNumber;
            }
        }

        protected void MergeOperationToMainDocument(int annexNumber)
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
    }
}