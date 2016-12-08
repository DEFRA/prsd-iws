namespace EA.Iws.Core.ImportNotification.Summary
{
    using System;
    using System.Collections.Generic;
    using ImportNotificationAssessment;
    using Shared;

    public class ImportNotificationSummary
    {
        public Guid Id { get; set; }

        public NotificationType Type { get; set; }

        public ImportNotificationStatus Status { get; set; }

        public string Number { get; set; }

        public Producer Producer { get; set; }

        public Exporter Exporter { get; set; }

        public Importer Importer { get; set; }

        public IList<Facility> Facilities { get; set; }

        public bool? AreFacilitiesPreconsented { get; set; }

        public StateOfExport StateOfExport { get; set; }

        public IList<TransitState> TransitStates { get; set; }

        public bool HasNoTransitStates { get; set; }

        public StateOfImport StateOfImport { get; set; }

        public IntendedShipment IntendedShipment { get; set; }

        public WasteOperation WasteOperation { get; set; }

        public WasteType WasteType { get; set; }

        public ChemicalComposition Composition { get; set; }
    }
}
