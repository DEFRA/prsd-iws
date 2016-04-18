namespace EA.Iws.DocumentGeneration.Notification.Blocks
{
    using System.Collections.Generic;
    using System.Linq;
    using DocumentFormat.OpenXml.Wordprocessing;
    using Mapper;
    using ViewModels;

    internal class NumberOfAnnexesAndInstructionsAndToCBlock
    {
        public ICollection<MergeField> CorrespondingMergeFields { get; private set; }
        private readonly NumberOfAnnexesAndInstructionsAndToCViewModel data;

        public NumberOfAnnexesAndInstructionsAndToCBlock(IList<MergeField> mergeFields, int annexNumber, string tocText, string instructionsText)
        {
            CorrespondingMergeFields = MergeFieldLocator.GetCorrespondingFieldsForBlock(mergeFields, TypeName);
            data = new NumberOfAnnexesAndInstructionsAndToCViewModel(tocText, instructionsText, annexNumber);
        }

        public void Merge()
        {
            var properties = PropertyHelper.GetPropertiesForViewModel(typeof(NumberOfAnnexesAndInstructionsAndToCViewModel));

            foreach (var field in CorrespondingMergeFields)
            {
                MergeFieldDataMapper.BindCorrespondingField(field, data, properties);
            }

            RemoveInstructionsTableIfEmpty();
        }

        public string TypeName
        {
            get { return "FinalBlock"; }
        }

        private void RemoveInstructionsTableIfEmpty()
        {
            if (string.IsNullOrEmpty(data.Instructions))
            {
                var instructionMergeField = CorrespondingMergeFields.Single(mf => mf.FieldName.InnerTypeName != null && mf.FieldName.InnerTypeName.Equals("Instructions"));
                var instructionsTable = instructionMergeField.Run.Ancestors<Table>().First();
                instructionsTable.Remove();
            }
        }
    }
}
