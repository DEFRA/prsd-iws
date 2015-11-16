namespace EA.Iws.Core.ImportNotification.Summary
{
    public class StateOfExport
    {
        public string CountryName { get; set; }

        public string CompetentAuthorityName { get; set; }

        public string CompetentAuthorityCode { get; set; }

        public string ExitPointName { get; set; }

        public bool IsEmpty()
        {
            return string.IsNullOrWhiteSpace(CountryName) &&
                   string.IsNullOrWhiteSpace(CompetentAuthorityCode) &&
                   string.IsNullOrWhiteSpace(CompetentAuthorityName) &&
                   string.IsNullOrWhiteSpace(ExitPointName);
        }
    }
}
