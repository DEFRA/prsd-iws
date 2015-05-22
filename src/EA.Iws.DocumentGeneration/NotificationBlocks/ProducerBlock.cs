namespace EA.Iws.DocumentGeneration.NotificationBlocks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using DocumentFormat.OpenXml.Wordprocessing;
    using Domain.Notification;
    using Mapper;
    using ViewModels;

    internal class ProducerBlock : INotificationBlock, IAnnexedBlock
    {
        private const string SiteOfExport = "SiteOfExport";
        private readonly IList<ProducerViewModel> data;

        public ProducerBlock(IList<MergeField> mergeFields, NotificationApplication notification)
        {
            CorrespondingMergeFields = MergeFieldLocator.GetCorrespondingFieldsForBlock(mergeFields, TypeName);

            var numberOfProducers = notification.Producers.Count();

            data = notification.Producers.Select(p => new ProducerViewModel(p, numberOfProducers)).ToList();

            // The producers annex contains a set of different merge fields for producer marked as Site of Export.
            AnnexMergeFields = MergeFieldLocator.GetAnnexMergeFields(mergeFields, TypeName);
            ((List<MergeField>)AnnexMergeFields).AddRange(MergeFieldLocator.GetAnnexMergeFields(mergeFields,
                SiteOfExport));
        }

        public IList<MergeField> AnnexMergeFields { get; private set; }

        public bool HasAnnex
        {
            get { return data.Count > 1; }
        }

        public void GenerateAnnex(int annexNumber)
        {
            var properties = PropertyHelper.GetPropertiesForViewModel(typeof(ProducerViewModel));

            // The main document should show "See Annex" for all data if there is an annex.
            MergeProducerToMainDocument(ProducerViewModel.GetProducerViewModelShowingSeeAnnexInstruction(annexNumber),
                properties);

            // Next merge the producer as the site of export.
            // TODO: Once the site of export property is available, use the producer marked as site of export.
            var siteOfExportProducer = data[0];
            MergeSiteOfExportInAnnex(siteOfExportProducer, properties);

            // Next remove the entry we have just merged so the multiple producers table doesn't contain this entry.
            data.RemoveAt(0);

            // If we only need the site of export in the annex clear the remaining annex fields and exit at this point.
            if (data.Count == 0)
            {
                ClearMultipleProducersTable();
                return;
            }

            // Finally merge the remaining producers into the multiple producers table and set the annex title.
            MergeMultipleProducersTable(properties);
            MergeAnnexNumber(annexNumber);
        }

        public ICollection<MergeField> CorrespondingMergeFields { get; private set; }

        public string TypeName
        {
            get { return "Producer"; }
        }

        public int OrdinalPosition
        {
            get { return 9; }
        }

        public void Merge()
        {
            var properties = PropertyHelper.GetPropertiesForViewModel(typeof(ProducerViewModel));

            // Where there is only 0 or 1 producers we don't need an annex.
            if (!HasAnnex)
            {
                if (data.Count == 1)
                {
                    MergeProducerToMainDocument(data[0], properties);
                }

                ClearAnnexFields();
            }
        }

        private void MergeProducerToMainDocument(ProducerViewModel producer, PropertyInfo[] properties)
        {
            foreach (var field in CorrespondingMergeFields)
            {
                MergeFieldDataMapper.BindCorrespondingField(field, producer, properties);
            }
        }

        /// <summary>
        ///     Merges all producers in the data list into the multiple producers table in the annex.
        /// </summary>
        /// <param name="properties">Property info for the view model.</param>
        private void MergeMultipleProducersTable(PropertyInfo[] properties)
        {
            var mergeTableRows = new TableRow[data.Count];

            // Find both the first row in the multiple producers table and the table itself.
            var firstMergeFieldInTable = FindFirstMergeFieldInAnnexTable();
            var table = FindMultipleProducersTable(firstMergeFieldInTable);

            // Get the table row containing the merge fields.
            mergeTableRows[0] = firstMergeFieldInTable.Run.Ancestors<TableRow>().Single();

            // Create a row containing merge fields for each of the producers.
            for (var i = 1; i < data.Count; i++)
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
                        MergeFieldLocator.ConvertAnnexMergeFieldToRegularMergeField(field), data[i], properties);
                }
            }
        }

        /// <summary>
        ///     Remove all fields for the producer annex.
        /// </summary>
        private void ClearAnnexFields()
        {
            ClearMultipleProducersTable();

            ClearAnnexTitleAndSeeAnnexNotice();

            ClearSiteOfExportFields();
        }

        private void ClearSiteOfExportFields()
        {
            foreach (var mergeField in FindSiteOfExportMergeFields())
            {
                mergeField.RemoveCurrentContents();
            }
        }

        private void ClearAnnexTitleAndSeeAnnexNotice()
        {
            FindAnnexNumberMergeField().RemoveCurrentContents();
        }

        private void ClearMultipleProducersTable()
        {
            var table = FindMultipleProducersTable(FindFirstMergeFieldInAnnexTable());
            table.Remove();
        }

        private void MergeSiteOfExportInAnnex(ProducerViewModel siteOfExportProducer, PropertyInfo[] properties)
        {
            foreach (var mergeField in FindSiteOfExportMergeFields())
            {
                MergeFieldDataMapper.BindCorrespondingField(mergeField, siteOfExportProducer, properties);
            }
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

        private Table FindMultipleProducersTable(MergeField firstMergeFieldInTable)
        {
            return firstMergeFieldInTable.Run.Ancestors<Table>().Single();
        }

        private IEnumerable<MergeField> FindSiteOfExportMergeFields()
        {
            return AnnexMergeFields.Where(mf => !string.IsNullOrWhiteSpace(mf.FieldName.OuterTypeName)
                                                && mf.FieldName.OuterTypeName.Equals(SiteOfExport));
        }
    }
}