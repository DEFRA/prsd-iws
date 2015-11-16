namespace EA.Iws.Core.ImportNotification.Summary
{
    public class StateOfImport
    {
        public string CompetentAuthorityName { get; set; }

        public string CompetentAuthorityCode { get; set; }

        public string EntryPointName { get; set; }

        public string CountryName
        {
            get { return "United Kingdom"; }
        }

        public bool IsEmpty()
        {
            return string.IsNullOrWhiteSpace(CompetentAuthorityCode) &&
                   string.IsNullOrWhiteSpace(CompetentAuthorityName) &&
                   string.IsNullOrWhiteSpace(EntryPointName);
        }
    }
}
