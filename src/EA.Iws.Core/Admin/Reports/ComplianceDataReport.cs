namespace EA.Iws.Core.Admin.Reports
{
    public class ComplianceDataReport : ComplianceData
    {
        public string EWCCode { get; set; }
        public string YCode { get; set; }

        public string PointOfExit { get; set; }
        public string PointOfEntry { get; set; }

        public string ExportCountryName { get; set; }
        public string ImportCountryName { get; set; }

        public string SiteOfExportName { get; set; }
        public string FacilityName { get; set; }
    }
}
