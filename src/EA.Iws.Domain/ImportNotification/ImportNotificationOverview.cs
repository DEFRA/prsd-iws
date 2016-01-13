namespace EA.Iws.Domain.ImportNotification
{
    using ImportNotificationAssessment;

    public class ImportNotificationOverview
    {
        public ImportNotification Notification { get; private set; }

        public ImportNotificationAssessment Assessment { get; private set; }

        public Exporter Exporter { get; private set; }

        public Importer Importer { get; private set; }

        public Producer Producer { get; private set; }

        public FacilityCollection Facilities { get; private set; }

        public Shipment Shipment { get; private set; }

        public TransportRoute TransportRoute { get; private set; }

        public WasteOperation WasteOperation { get; private set; }

        public WasteType WasteType { get; private set; }

        public static ImportNotificationOverview Load(ImportNotification notification,
            ImportNotificationAssessment assessment,
            Exporter exporter,
            Importer importer,
            Producer producer,
            FacilityCollection facilities,
            Shipment shipment,
            TransportRoute transportRoute,
            WasteOperation wasteOperation,
            WasteType wasteType)
        {
            return new ImportNotificationOverview
            {
                Notification = notification,
                Assessment = assessment,
                Exporter = exporter,
                Importer = importer,
                Producer = producer,
                Facilities = facilities,
                Shipment = shipment,
                TransportRoute = transportRoute,
                WasteOperation = wasteOperation,
                WasteType = wasteType
            };
        }
    }
}