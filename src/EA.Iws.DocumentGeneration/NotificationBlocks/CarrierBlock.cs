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

    internal class CarrierBlock : INotificationBlock, IAnnexedBlock
    {
        private readonly IList<CarrierViewModel> data;

        public CarrierBlock(IList<MergeField> mergeFields, NotificationApplication notification)
        {
            CorrespondingMergeFields = MergeFieldLocator.GetCorrespondingFieldsForBlock(mergeFields, TypeName);

            var motString = string.Empty;
            var mot = notification.MeansOfTransport.ToList();

            for (int i = 0; i < notification.MeansOfTransport.Count(); i++)
            {
                motString = motString + mot[i].Symbol;
                if (i != mot.Count - 1)
                {
                    motString = motString + " → ";
                }
            }

            data = notification.Carriers.Select(c => new CarrierViewModel(c, motString)).ToList();

            AnnexMergeFields = MergeFieldLocator.GetAnnexMergeFields(mergeFields, TypeName);
        }

        public bool HasAnnex 
        {
            get { return data.Count > 1; }
        }

        public void GenerateAnnex(int annexNumber)
        {
            var properties = PropertyHelper.GetPropertiesForViewModel(typeof(CarrierViewModel));

            MergeCarriersToMainDocument(CarrierViewModel.GetCarrierViewModelShowingSeeAnnexInstruction(annexNumber, data[0].MeansOfTransport), properties);

            MergeMultipleCarriersTable(properties);
            MergeAnnexNumber(annexNumber);
        }

        public IList<MergeField> AnnexMergeFields { get; private set; }

        private void MergeCarriersToMainDocument(CarrierViewModel carrier, PropertyInfo[] properties)
        {
            foreach (var field in CorrespondingMergeFields)
            {
                MergeFieldDataMapper.BindCorrespondingField(field, carrier, properties);
            }
        }

        private void MergeMultipleCarriersTable(PropertyInfo[] properties)
        {
            var mergeTableRows = new TableRow[data.Count];

            // Find both the first row in the multiple producers table and the table itself.
            var firstMergeFieldInTable = FindFirstMergeFieldInAnnexTable();
            var table = FindMultipleCarriersTable(firstMergeFieldInTable);

            // Get the table row containing the merge fields.
            mergeTableRows[0] = firstMergeFieldInTable.Run.Ancestors<TableRow>().Single();

            // Create a row containing merge fields for each of the Carriers.
            for (var i = 1; i < data.Count; i++)
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
                        MergeFieldLocator.ConvertAnnexMergeFieldToRegularMergeField(field), data[i], properties);
                }
            }
        }

        public ICollection<MergeField> CorrespondingMergeFields { get; private set; }

        public string TypeName
        {
            get { return "Carrier"; }
        }

        public int OrdinalPosition
        {
            get { return 8; }
        }

        public void Merge()
        {
            var carriers = PropertyHelper.GetPropertiesForViewModel(typeof(CarrierViewModel));

            if (!HasAnnex)
            {
                if (data.Count == 1)
                {
                    MergeCarriersToMainDocument(data[0], carriers);
                }

                ClearAnnexFields();
            }
        }

        private void ClearAnnexFields()
        {
            ClearMultipleCarriersTable();
            ClearAnnexTitleAndSeeAnnexNotice();
        }

        private void ClearAnnexTitleAndSeeAnnexNotice()
        {
            FindAnnexNumberMergeField().RemoveCurrentContents();
        }

        private void ClearMultipleCarriersTable()
        {
            var table = FindMultipleCarriersTable(FindFirstMergeFieldInAnnexTable());
            table.Remove();
        }

        private void MergeAnnexNumber(int annexNumber)
        {
            // Set the annex number as the page title.
            var annexNumberField = FindAnnexNumberMergeField();
            annexNumberField.RemoveCurrentContents();
            annexNumberField.SetText(annexNumber.ToString(), 0);
        }

        private MergeField FindFirstMergeFieldInAnnexTable()
        {
            return
                AnnexMergeFields.Single(
                    mf => mf.FieldName.OuterTypeName.Equals(TypeName, StringComparison.InvariantCultureIgnoreCase)
                          && mf.FieldName.InnerTypeName.Equals("RegistrationNumber",
                              StringComparison.InvariantCultureIgnoreCase));
        }

        private MergeField FindAnnexNumberMergeField()
        {
            return AnnexMergeFields.Single(
                mf => mf.FieldName.InnerTypeName != null && mf.FieldName.InnerTypeName.Equals("AnnexNumber"));
        }

        private Table FindMultipleCarriersTable(MergeField firstMergeFieldInTable)
        {
            return firstMergeFieldInTable.Run.Ancestors<Table>().Single();
        }
    }
}