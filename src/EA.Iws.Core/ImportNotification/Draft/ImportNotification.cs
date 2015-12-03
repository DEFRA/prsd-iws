namespace EA.Iws.Core.ImportNotification.Draft
{
    public class ImportNotification
    {
        public Exporter Exporter { get; set; }

        public FacilityCollection Facilities { get; set; }

        public Importer Importer { get; set; }

        public Preconsented Preconsented { get; set; }

        public Producer Producer { get; set; }

        public Shipment Shipment { get; set; }

        public StateOfExport StateOfExport { get; set; }

        public StateOfImport StateOfImport { get; set; }

        public TransitStateCollection TransitStates { get; set; }

        public WasteOperation WasteOperation { get; set; }

        public WasteType WasteType { get; set; }
    }
}
