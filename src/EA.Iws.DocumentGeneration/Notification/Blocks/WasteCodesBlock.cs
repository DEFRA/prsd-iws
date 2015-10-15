namespace EA.Iws.DocumentGeneration.Notification.Blocks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using DocumentFormat.OpenXml.Wordprocessing;
    using Domain.NotificationApplication;
    using Formatters;
    using Mapper;
    using ViewModels;

    internal class WasteCodesBlock : AnnexBlockBase, IDocumentBlock, IAnnexedBlock
    {
        private readonly WasteCodesViewModel data;

        public WasteCodesViewModel Data
        {
            get { return data; }
        }

        public ICollection<MergeField> CorrespondingMergeFields { get; private set; }

        public WasteCodesBlock(IList<MergeField> mergeFields, NotificationApplication notification)
        {
            CorrespondingMergeFields = MergeFieldLocator.GetCorrespondingFieldsForBlock(mergeFields, TypeName);
            data = new WasteCodesViewModel(notification, new WasteCodeInfoFormatter());

            AnnexMergeFields = MergeFieldLocator.GetAnnexMergeFields(mergeFields, TypeName);
        }

        public string TypeName
        {
            get { return "WasteCodes"; }
        }

        public virtual void Merge()
        {
            if (!HasAnnex)
            {
                var properties = PropertyHelper.GetPropertiesForViewModel(typeof(WasteCodesViewModel));

                foreach (var field in CorrespondingMergeFields)
                {
                    MergeFieldDataMapper.BindCorrespondingField(field, data, properties);
                }

                RemoveAnnex();
            }
        }

        public int OrdinalPosition
        {
            get { return 14; }
        }

        public bool HasAnnex
        {
            get { return data.IsAnnexNeeded; }
        }

        public virtual void GenerateAnnex(int annexNumber)
        {
            MergeToMainDocument(annexNumber);
            MergeAnnexNumber(annexNumber);

            TocText = "Annex " + annexNumber + " - Waste identification codes";

            var tableproperties = PropertyHelper.GetPropertiesForViewModel(typeof(AnnexTableWasteCodes));
            MergeToTable(tableproperties, data.AnnexTableWasteCodes);
        }

        protected void MergeToMainDocument(int annexNumber)
        {
            data.SetAnnexMessagesAndData(annexNumber);

            var properties = PropertyHelper.GetPropertiesForViewModel(typeof(WasteCodesViewModel));
            foreach (var field in CorrespondingMergeFields)
            {
                MergeFieldDataMapper.BindCorrespondingField(field, data, properties);
            }
        }

        protected void MergeToTable(PropertyInfo[] properties, IList<AnnexTableWasteCodes> list)
        {
            var mergeTableRows = new TableRow[list.Count()];

            var firstMergeFieldInTable = FindFirstMergeFieldInAnnexTable();
            var table = FindTable(firstMergeFieldInTable);

            mergeTableRows[0] = firstMergeFieldInTable.Run.Ancestors<TableRow>().First();

            for (var i = 1; i < list.Count(); i++)
            {
                mergeTableRows[i] = (TableRow)mergeTableRows[0].CloneNode(true);
                table.AppendChild(mergeTableRows[i]);
            }

            for (var i = 0; i < mergeTableRows.Length; i++)
            {
                foreach (var field in MergeFieldLocator.GetMergeRuns(mergeTableRows[i]))
                {
                    MergeFieldDataMapper.BindCorrespondingField(
                        MergeFieldLocator.ConvertAnnexMergeFieldToRegularMergeField(field), list[i], properties);
                }
            }
        }

        protected MergeField FindFirstMergeFieldInAnnexTable()
        {
            return
                AnnexMergeFields.Single(
                    mf => mf.FieldName.OuterTypeName.Equals(TypeName, StringComparison.InvariantCultureIgnoreCase)
                          && mf.FieldName.InnerTypeName.Equals("Name",
                              StringComparison.InvariantCultureIgnoreCase));
        }

        protected Table FindTable(MergeField firstMergeFieldInTable)
        {
            var fieldRun = firstMergeFieldInTable.Run;
            var tableAncestors = fieldRun.Ancestors<Table>();
            return tableAncestors.First();
        }
    }
}
