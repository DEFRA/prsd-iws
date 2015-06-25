namespace EA.Iws.Core.TransitState
{
    using Shared;
    using TransportRoute;

    public class TransitStateData
    {
        public CountryData Country { get; set; }

        public CompetentAuthorityData CompetentAuthority { get; set; }

        public EntryOrExitPointData EntryPoint { get; set; }

        public EntryOrExitPointData ExitPoint { get; set; }

        public int OrdinalPosition { get; set; }
    }
}