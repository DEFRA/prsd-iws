namespace EA.Iws.Requests.TransitState
{
    using System.Collections.Generic;
    using Core.Shared;
    using Core.StateOfExport;
    using Core.StateOfImport;
    using Core.TransitState;
    using Core.TransportRoute;

    public class TransitStateWithTransportRouteData
    {
        public StateOfExportData StateOfExport { get; set; }

        public TransitStateData TransitState { get; set; }

        public IList<TransitStateData> TransitStates { get; set; }

        public StateOfImportData StateOfImport { get; set; }

        public CountryData[] Countries { get; set; }

        public EntryOrExitPointData[] EntryOrExitPoints { get; set; }

        public CompetentAuthorityData[] CompetentAuthorities { get; set; }

        public TransitStateWithTransportRouteData()
        {
            TransitStates = new List<TransitStateData>();
        }
    }
}
