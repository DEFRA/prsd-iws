namespace EA.Iws.DocumentGeneration.Notification.Blocks
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
        protected const string ActualSiteOfTreatment = "ActualSite";
        private readonly IList<FacilityViewModel> data;

        public IReadOnlyList<FacilityViewModel> Data
        {
            get
            {
                return data.ToList();
            }
        }

        public FacilityBlock(IList<MergeField> mergeFields, FacilityCollection facilityCollection)
        {
            CorrespondingMergeFields = MergeFieldLocator.GetCorrespondingFieldsForBlock(mergeFields, TypeName);

            // Facilities are already ordered by OrdinalPosition from the domain.
            var numberOfFacilities = facilityCollection.Facilities.Count();
            data = facilityCollection.Facilities.Select(p => new FacilityViewModel(p, numberOfFacilities)).ToList();

            //The facility annex contains a set of different merge fields for facility marked as Actual Site of Treatment.
            AnnexMergeFields = MergeFieldLocator.GetAnnexMergeFields(mergeFields, TypeName);
            ((List<MergeField>)AnnexMergeFields).AddRange(MergeFieldLocator.GetAnnexMergeFields(mergeFields, ActualSiteOfTreatment));
        }

        public bool HasAnnex
        {
            get { return data.Count > 1; }
        }

        public virtual void GenerateAnnex(int annexNumber)
        {
            var properties = PropertyHelper.GetPropertiesForViewModel(typeof(FacilityViewModel));

            TocText = "Annex " + annexNumber + " - Disposal or recovery facility";

            // Always display the first facility (by ordinal position) in Block 10
            // with full name and address, per regulatory requirements.
            var firstFacility = data[0];
            MergeFacilityToMainDocument(FacilityViewModel.GetFirstFacilityWithAnnexReference(firstFacility, annexNumber), properties);

            // Remove the first facility so it does not appear again in the annex.
            data.RemoveAt(0);

            // Find and merge the actual site of treatment into the annex.
            // If the first facility was also the actual site, it has already been
            // removed from data, so use the original reference instead.
            if (firstFacility.IsActualSite)
            {
                MergeActualSiteOfTreatmentInAnnex(firstFacility, properties);
            }
            else
            {
                var indexOfActualSite = data.Where(x => x.IsActualSite).Select(data.IndexOf).Single();
                MergeActualSiteOfTreatmentInAnnex(data[indexOfActualSite], properties);
                data.RemoveAt(indexOfActualSite);
            }

            MergeAnnexNumber(annexNumber);

            // If there are no remaining facilities beyond the actual site, clear the table.
            if (data.Count < 1)
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

        public virtual void Merge()
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

        protected void MergeFacilityToMainDocument(FacilityViewModel facility, PropertyInfo[] properties)
        {
            foreach (var field in CorrespondingMergeFields)
            {
                MergeFieldDataMapper.BindCorrespondingField(field, facility, properties);
            }
        }

        protected void MergeMultipleFacilitiesTable(PropertyInfo[] properties)
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

        protected void ClearMultipleFacilitiesTable()
        {
            var table = FindMultipleFacilitiesTable(FindFirstMergeFieldInAnnexTable());
            table.Remove();
        }

        protected void MergeActualSiteOfTreatmentInAnnex(FacilityViewModel actualSiteOfTreatmentFacility, PropertyInfo[] properties)
        {
            foreach (var mergeField in FindActualSiteOfTreatmentMergeFields())
            {
                MergeFieldDataMapper.BindCorrespondingField(mergeField, actualSiteOfTreatmentFacility, properties);
            }
        }

        protected MergeField FindFirstMergeFieldInAnnexTable()
        {
            return AnnexMergeFields.Single(mf => mf.FieldName.OuterTypeName.Equals(TypeName, StringComparison.InvariantCultureIgnoreCase)
                          && mf.FieldName.InnerTypeName.Equals("RegistrationNumber", StringComparison.InvariantCultureIgnoreCase));
        }

        protected Table FindMultipleFacilitiesTable(MergeField firstMergeFieldInTable)
        {
            return firstMergeFieldInTable.Run.Ancestors<Table>().First();
        }

        protected IEnumerable<MergeField> FindActualSiteOfTreatmentMergeFields()
        {
            return AnnexMergeFields.Where(mf => !string.IsNullOrWhiteSpace(mf.FieldName.OuterTypeName)
                                                && mf.FieldName.OuterTypeName.Equals(ActualSiteOfTreatment));
        }
    }
}