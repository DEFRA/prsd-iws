namespace EA.Iws.Requests.StateOfImport
{
    using Registration;
    using Shared;
    using TransportRoute;

    public class StateOfImportData
    {
        public CountryData Country { get; set; }

        public CompetentAuthorityData CompetentAuthority { get; set; }

        public EntryOrExitPointData EntryPoint { get; set; }
    }
}