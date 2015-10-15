namespace EA.Iws.DocumentGeneration.Notification.Blocks
{
    using System.Collections.Generic;
    using System.Linq;
    using DocumentFormat.OpenXml.Wordprocessing;

    internal abstract class AnnexBlockBase
    {
        public IList<MergeField> AnnexMergeFields { get; protected set; }
        public string TocText { get; protected set; }
        public string InstructionsText { get; protected set; }

        protected void MergeAnnexNumber(int annexNumber)
        {
            var annexNumberField = FindAnnexNumberMergeField();
            annexNumberField.RemoveCurrentContents();
            annexNumberField.SetText(annexNumber.ToString(), 0);
        }

        protected MergeField FindAnnexNumberMergeField()
        {
            return AnnexMergeFields.Single(
                mf => mf.FieldName.InnerTypeName != null && mf.FieldName.InnerTypeName.Equals("AnnexNumber"));
        }

        protected void RemoveAnnex()
        {
            var table = FindAnnexNumberMergeField().Run.Ancestors<Table>().Single();
            table.Remove();
        }
    }
}
