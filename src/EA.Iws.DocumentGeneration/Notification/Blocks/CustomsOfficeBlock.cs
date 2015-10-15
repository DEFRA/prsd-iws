namespace EA.Iws.DocumentGeneration.Notification.Blocks
{
    using System.Collections.Generic;
    using Domain.TransportRoute;
    using Mapper;
    using ViewModels;

    internal class CustomsOfficeBlock : AnnexBlockBase, IDocumentBlock, IAnnexedBlock
    {
        private readonly CustomsOfficeViewModel data;

        public ICollection<MergeField> CorrespondingMergeFields { get; private set; }
        //public IList<MergeField> AnnexMergeFields { get; private set; }

        public CustomsOfficeBlock(IList<MergeField> mergeFields, TransportRoute transportRoute)
        {
            CorrespondingMergeFields = MergeFieldLocator.GetCorrespondingFieldsForBlock(mergeFields, TypeName);
            data = new CustomsOfficeViewModel(transportRoute);

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

                RemoveAnnex();
            }
        }

        public void GenerateAnnex(int annexNumber)
        {
            MergeToMainDocument(annexNumber);

            TocText = "Annex " + annexNumber + " - Customs offices";

            var properties = PropertyHelper.GetPropertiesForViewModel(typeof(CustomsOfficeViewModel));

            foreach (var field in AnnexMergeFields)
            {
                MergeFieldDataMapper.BindCorrespondingField(field, data, properties);
            }

            MergeAnnexNumber(annexNumber);
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
    }
}
