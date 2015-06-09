namespace EA.Iws.Requests.TransportRoute
{
    using System.Collections.Generic;
    using StateOfExport;
    using StateOfImport;

    public class TransportRouteData
    {
        public StateOfExportData StateOfExportData { get; set; }

        public IList<TransitStateData> TransitStatesData { get; set; }

        public StateOfImportData StateOfImportData { get; set; }
    }
}