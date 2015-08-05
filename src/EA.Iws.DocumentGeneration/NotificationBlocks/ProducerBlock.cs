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

    internal class ProducerBlock : AnnexBlockBase, INotificationBlock, IAnnexedBlock
    {
        private const string SiteOfExport = "SiteOfExport";
        private const string PoGtext = "PoGtext";
        private readonly IList<ProducerViewModel> data;

        public ProducerBlock(IList<MergeField> mergeFields, NotificationApplication notification)
        {
            CorrespondingMergeFields = MergeFieldLocator.GetCorrespondingFieldsForBlock(mergeFields, TypeName);

            var numberOfProducers = notification.Producers.Count();
            var processText = notification.WasteGenerationProcess;
            var isProcessAnnexAttached = notification.IsWasteGenerationProcessAttached;

            data = notification.Producers.Select(p => new ProducerViewModel(p, numberOfProducers, processText, isProcessAnnexAttached)).ToList();

            // The producers annex contains a set of different merge fields for producer marked as Site of Export.
            AnnexMergeFields = MergeFieldLocator.GetAnnexMergeFields(mergeFields, TypeName);
            ((List<MergeField>)AnnexMergeFields).AddRange(MergeFieldLocator.GetAnnexMergeFields(mergeFields,
                SiteOfExport));
            ((List<MergeField>)AnnexMergeFields).AddRange(MergeFieldLocator.GetAnnexMergeFields(mergeFields,
                PoGtext));
        }

        public bool HasAnnex
        {
            get
            {
                bool isProcessAnnexAttached = data[0].IsProcessAnnexAttached.GetValueOrDefault();
                return (data.Count > 1 || isProcessAnnexAttached || data[0].ProcessOfGeneration.Length > ProducerViewModel.ProcessOfGenerationMaxTextLength());
            }
        }

        public void GenerateAnnex(int annexNumber)
        {
            var properties = PropertyHelper.GetPropertiesForViewModel(typeof(ProducerViewModel));
            MergeAnnexNumber(annexNumber);
            MergeProcessOfGenerationTextInAnnex(data[0], properties);

            TocText = "Annex " + annexNumber + " - Waste generators - producers";

            if (data[0].IsProcessAnnexAttached.GetValueOrDefault())
            {
                InstructionsText = "Process of generation - annex " + annexNumber;
            }

            //If there is only one producer but also an annex
            if (data.Count == 1)
            {
                MergeProducerToMainDocument(data[0].GetProducerViewModelShowingAnnexMessages(data.Count, data[0], annexNumber), properties);
                RemoveSiteOfGenerationTable();
                ClearMultipleProducersTable();
            }

            //If there are two producers the one that is the site of generation goes in the annex
            //and the other goes on the notification document in block 9
            if (data.Count > 1)
            {
                //Merge site of generation to annex
                var indexOfSiteOfGeneration = data.Where(x => x.IsSiteOfGeneration).Select(data.IndexOf).Single();
                MergeSiteOfExportInAnnex(data[indexOfSiteOfGeneration], properties);

                // Next remove the entry we have just merged so the multiple producers table doesn't contain this entry.
                data.RemoveAt(indexOfSiteOfGeneration);

                // If we only need the site of generation in the annex clear the remaining annex fields and exit at this point.
                if (data.Count < 2)
                {
                    ClearMultipleProducersTable();
                }

                // Merge the remaining producers into the multiple producers table and set the annex title.
                MergeMultipleProducersTable(properties);

                //If there is only one left put it on the form otherwise put them all in the annex
                MergeProducerToMainDocument(data[0].GetProducerViewModelShowingAnnexMessages(data.Count, data[0], annexNumber), properties);
            }

            //Clear the annex process of generation field if it contains no text
            if (!(data[0].ProcessOfGeneration.Length > ProducerViewModel.ProcessOfGenerationMaxTextLength()))
            {
                ClearProcessOfGenerationTextFields();
            }
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

            // Where there is 0 or 1 producers we don't need an annex but there could be a message for process of generation.
            if (!HasAnnex)
            {
                if (data.Count == 1)
                {
                    MergeProducerToMainDocument(data[0].GetProducerViewModelShowingAnnexMessages(data.Count, data[0], 0), properties);
                }

                RemoveAnnex();
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
            mergeTableRows[0] = firstMergeFieldInTable.Run.Ancestors<TableRow>().First();

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

        private void ClearProcessOfGenerationTextFields()
        {
            foreach (var mergeField in FindProcessOfGenerationTextMergeFields())
            {
                mergeField.RemoveCurrentContents();
            }
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

        private void MergeProcessOfGenerationTextInAnnex(ProducerViewModel pvm, PropertyInfo[] proerties)
        {
            if (pvm.ProcessOfGeneration.Length > ProducerViewModel.ProcessOfGenerationMaxTextLength())
            {
                foreach (var mergeField in FindProcessOfGenerationTextMergeFields())
                {
                    MergeFieldDataMapper.BindCorrespondingField(mergeField, pvm, proerties);
                }
            }
        }

        private MergeField FindFirstMergeFieldInAnnexTable()
        {
            return
                AnnexMergeFields.Single(
                    mf => mf.FieldName.OuterTypeName.Equals(TypeName, StringComparison.InvariantCultureIgnoreCase)
                          && mf.FieldName.InnerTypeName.Equals("RegistrationNumber",
                              StringComparison.InvariantCultureIgnoreCase));
        }

        private Table FindMultipleProducersTable(MergeField firstMergeFieldInTable)
        {
            return firstMergeFieldInTable.Run.Ancestors<Table>().First();
        }

        private void RemoveSiteOfGenerationTable()
        {
            MergeField f = AnnexMergeFields.Single(
                    mf => mf.FieldName.OuterTypeName.Equals("SiteOfExport", StringComparison.InvariantCultureIgnoreCase)
                          && mf.FieldName.InnerTypeName.Equals("RegistrationNumber",
                              StringComparison.InvariantCultureIgnoreCase));

            f.Run.Ancestors<Table>().First().Remove();
        }

        private IEnumerable<MergeField> FindSiteOfExportMergeFields()
        {
            return AnnexMergeFields.Where(mf => !string.IsNullOrWhiteSpace(mf.FieldName.OuterTypeName)
                                                && mf.FieldName.OuterTypeName.Equals(SiteOfExport));
        }

        private IEnumerable<MergeField> FindProcessOfGenerationTextMergeFields()
        {
            return AnnexMergeFields.Where(mf => !string.IsNullOrWhiteSpace(mf.FieldName.OuterTypeName)
                                                && mf.FieldName.OuterTypeName.Equals(PoGtext));
        }
    }
}