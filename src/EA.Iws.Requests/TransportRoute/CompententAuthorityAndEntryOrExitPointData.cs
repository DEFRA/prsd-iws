namespace EA.Iws.Requests.TransportRoute
{
    using Core.Shared;
    using Core.TransportRoute;

    public class CompententAuthorityAndEntryOrExitPointData
    {
        public CompetentAuthorityData[] CompetentAuthorities { get; set; }
        public EntryOrExitPointData[] EntryOrExitPoints { get; set; }
    }
}
