namespace EA.Iws.Core.StateOfExport
{
    using System.Collections.Generic;
    using Shared;
    using StateOfImport;
    using TransitState;
    using TransportRoute;

    public class StateOfExportWithTransportRouteData
    {
        public StateOfExportData StateOfExport { get; set; }

        public IList<TransitStateData> TransitStates { get; set; }

        public StateOfImportData StateOfImport { get; set; }

        public CountryData[] Countries { get; set; }

        public EntryOrExitPointData[] ExitPoints { get; set; }

        public CompetentAuthorityData[] CompetentAuthorities { get; set; }

        public StateOfExportWithTransportRouteData()
        {
            TransitStates = new List<TransitStateData>();
        }
    }
}
