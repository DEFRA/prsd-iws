namespace EA.Iws.Core.StateOfImport
{
    using Shared;
    using TransportRoute;

    public class StateOfImportData
    {
        public CountryData Country { get; set; }

        public CompetentAuthorityData CompetentAuthority { get; set; }

        public EntryOrExitPointData EntryPoint { get; set; }
    }
}