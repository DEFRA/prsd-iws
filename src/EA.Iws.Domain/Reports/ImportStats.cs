namespace EA.Iws.Domain.Reports
{
    public class ImportStats
    {
        public decimal QuantityReceived { get; protected set; }

        public string WasteStreams { get; protected set; }

        public string WasteCategory { get; protected set; }

        public string CountryOfExport { get; protected set; }

        public string TransitStates { get; protected set; }

        public string BaselOecd { get; protected set; }

        public string Ewc { get; protected set; }

        public string HCode { get; protected set; }

        public string HCodeDescription { get; protected set; }

        public string UN { get; protected set; }

        public string RCode { get; protected set; }

        public string DCode { get; protected set; }
    }
}