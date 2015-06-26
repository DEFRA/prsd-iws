namespace EA.Iws.Requests.StateOfExport
{
    using System.Collections.Generic;
    using Core.Shared;
    using Core.StateOfExport;
    using Core.StateOfImport;
    using Core.TransitState;
    using Core.TransportRoute;

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
