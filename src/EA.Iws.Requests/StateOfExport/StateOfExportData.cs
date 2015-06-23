namespace EA.Iws.Requests.StateOfExport
{
    using Core.Shared;
    using Shared;
    using TransportRoute;

    public class StateOfExportData
    {
        public CountryData Country { get; set; }

        public CompetentAuthorityData CompetentAuthority { get; set; }

        public EntryOrExitPointData ExitPoint { get; set; }
    }
}