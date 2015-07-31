namespace EA.Iws.DocumentGeneration.NotificationBlocks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using DocumentFormat.OpenXml.Wordprocessing;
    using Domain.NotificationApplication;
    using Mapper;
    using ViewModels;

    internal class TransitStatesBlock : INotificationBlock, IAnnexedBlock
    {
        private readonly TransitStateViewModel data;

        public IList<MergeField> AnnexMergeFields { get; private set; }
        public ICollection<MergeField> CorrespondingMergeFields { get; private set; }

        public TransitStatesBlock(IList<MergeField> mergeFields, NotificationApplication notification)
        {
            CorrespondingMergeFields = MergeFieldLocator.GetCorrespondingFieldsForBlock(mergeFields, TypeName);
            data = new TransitStateViewModel(notification.TransitStates.ToList());

            AnnexMergeFields = MergeFieldLocator.GetAnnexMergeFields(mergeFields, TypeName);
        }

        public string TypeName
        {
            get { return "Transits"; }
        }

        public int OrdinalPosition
        {
            get { return 15; }
        }

        public void Merge()
        {
            if (!HasAnnex)
            {
                MergeToMainDocument();

                ClearAnnexTable();
            }
        }

        public bool HasAnnex
        {
            get { return data.IsAnnexNeeded; }
        }

        private void MergeToMainDocument()
        {
            var properties = PropertyHelper.GetPropertiesForViewModel(typeof(TransitStateViewModel));
            foreach (var field in CorrespondingMergeFields)
            {
                MergeFieldDataMapper.BindCorrespondingField(field, data, properties);
            }
        }

        public void GenerateAnnex(int annexNumber)
        {
            data.SetAnnexMessage(annexNumber);
            MergeToMainDocument();
            MergeAnnexNumber(annexNumber);

            var tableproperties = PropertyHelper.GetPropertiesForViewModel(typeof(TransitStateDetail));
            MergeToTable(tableproperties);
        }

        private void MergeToTable(PropertyInfo[] properties)
        {
            var mergeTableRows = new TableRow[data.TransitStateDetails.Count];

            // Find both the first row in the multiple producers table and the table itself.
            var firstMergeFieldInTable = FindFirstMergeFieldInAnnexTable();
            var table = FindAnnexTable(firstMergeFieldInTable);

            // Get the table row containing the merge fields.
            mergeTableRows[0] = firstMergeFieldInTable.Run.Ancestors<TableRow>().Single();

            // Create a row containing merge fields for each of the producers.
            for (var i = 1; i < data.TransitStateDetails.Count; i++)
            {
                mergeTableRows[i] = (TableRow)mergeTableRows[0].CloneNode(true);
                table.AppendChild(mergeTableRows[i]);
            }

            // Merge the producers into the table rows.
            for (var i = 0; i < mergeTableRows.Length; i++)
            {
                foreach (var field in MergeFieldLocator.GetMergeRuns(mergeTableRows[i]))
                {
                    MergeFieldDataMapper.BindCorrespondingField(
                        MergeFieldLocator.ConvertAnnexMergeFieldToRegularMergeField(field), data.TransitStateDetails[i], properties);
                }
            }
        }

        private void ClearAnnexTable()
        {
            var table = FindAnnexTable(FindFirstMergeFieldInAnnexTable());
            table.Remove();
        }

        private MergeField FindFirstMergeFieldInAnnexTable()
        {
            return
                AnnexMergeFields.Single(
                    mf => mf.FieldName.OuterTypeName.Equals(TypeName, StringComparison.InvariantCultureIgnoreCase)
                          && mf.FieldName.InnerTypeName.Equals("Country",
                              StringComparison.InvariantCultureIgnoreCase));
        }

        private Table FindAnnexTable(MergeField firstMergeFieldInTable)
        {
            return firstMergeFieldInTable.Run.Ancestors<Table>().Single();
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
            return AnnexMergeFields.Single(
                mf => mf.FieldName.InnerTypeName != null && mf.FieldName.InnerTypeName.Equals("AnnexNumber"));
        }
    }
}
