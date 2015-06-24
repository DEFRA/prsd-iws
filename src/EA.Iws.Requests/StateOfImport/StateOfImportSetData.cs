namespace EA.Iws.Requests.StateOfImport
{
    using System.Collections.Generic;
    using Core.Shared;
    using Core.StateOfExport;
    using Core.StateOfImport;
    using Core.TransitState;
    using Core.TransportRoute;

    public class StateOfImportSetData
    {
        public StateOfExportData StateOfExport { get; set; }

        public IList<TransitStateData> TransitStates { get; set; }

        public StateOfImportData StateOfImport { get; set; }

        public CountryData[] Countries { get; set; }

        public EntryOrExitPointData[] EntryPoints { get; set; }

        public CompetentAuthorityData[] CompetentAuthorities { get; set; }

        public StateOfImportSetData()
        {
            TransitStates = new List<TransitStateData>();
        }
    }
}
