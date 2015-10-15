namespace EA.Iws.DocumentGeneration.Movement.Blocks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DocumentFormat.OpenXml.Wordprocessing;
    using Domain.Movement;
    using Mapper;
    using ViewModels;

    public class MovementCarrierBlock : IDocumentBlock
    {
        private readonly MovementCarriersViewModel data;

        public MovementCarrierBlock(IList<MergeField> mergeFields, Movement movement)
        {
            CorrespondingMergeFields = MergeFieldLocator.GetCorrespondingFieldsForBlock(mergeFields, TypeName);
            data = new MovementCarriersViewModel(movement.MovementCarriers.ToList());
            AnnexMergeFields = MergeFieldLocator.GetAnnexMergeFields(mergeFields, TypeName);
        }

        public string TypeName 
        {
            get { return "Carrier"; }
        }

        public ICollection<MergeField> CorrespondingMergeFields { get; private set; }

        public bool HasAnnex 
        {
            get { return data.IsAnnexNeeded; }
        }

        public void Merge()
        {
            var properties = PropertyHelper.GetPropertiesForViewModel(typeof(MovementCarriersViewModel));

            foreach (var field in CorrespondingMergeFields)
            {
                MergeFieldDataMapper.BindCorrespondingField(field, data, properties);
            }

            if (HasAnnex)
            {
                MergeCarriersTable();
            }
            else
            {
                RemoveAnnex();
            }
        }

        public IList<MergeField> AnnexMergeFields { get; protected set; }

        public int OrdinalPosition 
        {
            get { return 8; }
        }

        protected void RemoveAnnex()
        {
            var field = AnnexMergeFields.Single(
                mf => mf.FieldName.InnerTypeName != null && mf.FieldName.InnerTypeName.Equals("Order"));

            var table = field.Run.Ancestors<Table>().First().Ancestors<Table>().First();
            table.Remove();
        }

        private void MergeCarriersTable()
        {
            var mergeTableRows = new TableRow[data.CarrierDetails.Count];
            var properties = PropertyHelper.GetPropertiesForViewModel(typeof(MovementCarrierDetails));

            // Find both the first row in the multiple carriers table and the table itself.
            var firstMergeFieldInTable = FindFirstMergeFieldInAnnexTable();
            var table = FindMultipleCarriersTable(firstMergeFieldInTable);

            // Get the table row containing the merge fields.
            mergeTableRows[0] = firstMergeFieldInTable.Run.Ancestors<TableRow>().First();

            // Create a row containing merge fields for each of the Carriers.
            for (var i = 1; i < data.CarrierDetails.Count; i++)
            {
                mergeTableRows[i] = (TableRow)mergeTableRows[0].CloneNode(true);
                table.AppendChild(mergeTableRows[i]);
            }

            // Merge the carriers into the table rows.
            for (var i = 0; i < mergeTableRows.Length; i++)
            {
                foreach (var field in MergeFieldLocator.GetMergeRuns(mergeTableRows[i]))
                {
                    MergeFieldDataMapper.BindCorrespondingField(
                        MergeFieldLocator.ConvertAnnexMergeFieldToRegularMergeField(field), data.CarrierDetails[i], properties);
                }
            }
        }

        private MergeField FindFirstMergeFieldInAnnexTable()
        {
            return
                AnnexMergeFields.Single(
                    mf => mf.FieldName.OuterTypeName.Equals(TypeName, StringComparison.InvariantCultureIgnoreCase)
                          && mf.FieldName.InnerTypeName.Equals("Order",
                              StringComparison.InvariantCultureIgnoreCase));
        }

        private Table FindMultipleCarriersTable(MergeField firstMergeFieldInTable)
        {
            return firstMergeFieldInTable.Run.Ancestors<Table>().First();
        }
    }
}
