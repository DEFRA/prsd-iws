namespace EA.Iws.Core.TransportRoute
{
    using System.Collections.Generic;
    using Shared;

    public class EntryOrExitPointDataGroup
    {
        public CountryData Country { get; set; }

        public IList<EntryOrExitPointData> EntryOrExitPoints { get; set; }
    }
}
