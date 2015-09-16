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

    internal class FacilityBlock : AnnexBlockBase, IDocumentBlock, IAnnexedBlock
    {
        private const string ActualSiteOfTreatment = "ActualSite";
        private readonly IList<FacilityViewModel> data;

        public FacilityBlock(IList<MergeField> mergeFields, NotificationApplication notification)
        {
            CorrespondingMergeFields = MergeFieldLocator.GetCorrespondingFieldsForBlock(mergeFields, TypeName);

            var numberOfFacilities = notification.Facilities.Count();
            data = notification.Facilities.Select(p => new FacilityViewModel(p, numberOfFacilities)).ToList();

            //The facility annex contains a set of different merge fields for facility marked as Actual Site of Treatment.
            AnnexMergeFields = MergeFieldLocator.GetAnnexMergeFields(mergeFields, TypeName);
            ((List<MergeField>)AnnexMergeFields).AddRange(MergeFieldLocator.GetAnnexMergeFields(mergeFields, ActualSiteOfTreatment));
        }

        public bool HasAnnex
        {
            get { return data.Count > 1; }
        }

        public void GenerateAnnex(int annexNumber)
        {
            var properties = PropertyHelper.GetPropertiesForViewModel(typeof(FacilityViewModel));

            TocText = "Annex " + annexNumber + " - Disposal or recovery facility";

            if (data.Count == 2)
            {
                //if the user enters two facilities:
                //the facility that is the "actual site" goes into an annex
                //the other site goes onto the notification document in block 10 
                if (data[0].IsActualSite)
                {
                    MergeFacilityToMainDocument(FacilityViewModel.GetSeeAnnexInstructionForFacilityCaseTwoFacilities(data[1], annexNumber), properties);
                }
                else
                {
                    MergeFacilityToMainDocument(FacilityViewModel.GetSeeAnnexInstructionForFacilityCaseTwoFacilities(data[0], annexNumber), properties);
                }
            }
            else
            {
                //The main document should show "See Annex" for all data if there is an annex.
                MergeFacilityToMainDocument(FacilityViewModel.GetSeeAnnexInstructionForFacility(annexNumber), properties);
            }

            var indexOfActualSite = data.Where(x => x.IsActualSite).Select(data.IndexOf).Single();
            MergeActualSiteOfTreatmentInAnnex(data[indexOfActualSite], properties);
            data.RemoveAt(indexOfActualSite);

            MergeAnnexNumber(annexNumber);

            //If we only need actual site of treatment in the annex, clear the remaining annex fields and exit at this point.
            if (data.Count < 2)
            {
                ClearMultipleFacilitiesTable();
                return;
            }

            //Finally merge the remaining facilities into the multiple facilities table and set the annex title.
            MergeMultipleFacilitiesTable(properties);
        }

        public ICollection<MergeField> CorrespondingMergeFields { get; private set; }

        public string TypeName
        {
            get { return "Facility"; }
        }

        public int OrdinalPosition
        {
            get { return 10; }
        }

        public void Merge()
        {
            var properties = PropertyHelper.GetPropertiesForViewModel(typeof(FacilityViewModel));

            if (!HasAnnex)
            {
                if (data.Count == 1)
                {
                    MergeFacilityToMainDocument(data[0], properties);
                }

                RemoveAnnex();
            }
        }

        private void MergeFacilityToMainDocument(FacilityViewModel facility, PropertyInfo[] properties)
        {
            foreach (var field in CorrespondingMergeFields)
            {
                MergeFieldDataMapper.BindCorrespondingField(field, facility, properties);
            }
        }

        private void MergeMultipleFacilitiesTable(PropertyInfo[] properties)
        {
            var mergeTableRows = new TableRow[data.Count];
            var firstMergeFieldInTable = FindFirstMergeFieldInAnnexTable();
            var table = FindMultipleFacilitiesTable(firstMergeFieldInTable);

            // Get the table row containing the merge fields.
            mergeTableRows[0] = firstMergeFieldInTable.Run.Ancestors<TableRow>().First();
            for (var i = 1; i < data.Count; i++)
            {
                mergeTableRows[i] = (TableRow)mergeTableRows[0].CloneNode(true);
                table.AppendChild(mergeTableRows[i]);
            }

            for (var i = 0; i < mergeTableRows.Length; i++)
            {
                foreach (var field in MergeFieldLocator.GetMergeRuns(mergeTableRows[i]))
                {
                    MergeFieldDataMapper.BindCorrespondingField(
                        MergeFieldLocator.ConvertAnnexMergeFieldToRegularMergeField(field), data[i], properties);
                }
            }
        }

        private void ClearMultipleFacilitiesTable()
        {
            var table = FindMultipleFacilitiesTable(FindFirstMergeFieldInAnnexTable());
            table.Remove();
        }

        private void MergeActualSiteOfTreatmentInAnnex(FacilityViewModel actualSiteOfTreatmentFacility, PropertyInfo[] properties)
        {
            foreach (var mergeField in FindActualSiteOfTreatmentMergeFields())
            {
                MergeFieldDataMapper.BindCorrespondingField(mergeField, actualSiteOfTreatmentFacility, properties);
            }
        }

        private MergeField FindFirstMergeFieldInAnnexTable()
        {
            return AnnexMergeFields.Single(mf => mf.FieldName.OuterTypeName.Equals(TypeName, StringComparison.InvariantCultureIgnoreCase)
                          && mf.FieldName.InnerTypeName.Equals("RegistrationNumber", StringComparison.InvariantCultureIgnoreCase));
        }

        private Table FindMultipleFacilitiesTable(MergeField firstMergeFieldInTable)
        {
            return firstMergeFieldInTable.Run.Ancestors<Table>().First();
        }

        private IEnumerable<MergeField> FindActualSiteOfTreatmentMergeFields()
        {
            return AnnexMergeFields.Where(mf => !string.IsNullOrWhiteSpace(mf.FieldName.OuterTypeName)
                                                && mf.FieldName.OuterTypeName.Equals(ActualSiteOfTreatment));
        }
    }
}