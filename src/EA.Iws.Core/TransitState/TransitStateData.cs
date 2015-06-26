namespace EA.Iws.Core.TransitState
{
    using System;
    using Shared;
    using TransportRoute;

    public class TransitStateData
    {
        public Guid Id { get; set; }

        public CountryData Country { get; set; }

        public CompetentAuthorityData CompetentAuthority { get; set; }

        public EntryOrExitPointData EntryPoint { get; set; }

        public EntryOrExitPointData ExitPoint { get; set; }

        public int OrdinalPosition { get; set; }
    }
}