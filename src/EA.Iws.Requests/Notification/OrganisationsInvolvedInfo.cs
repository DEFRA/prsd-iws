namespace EA.Iws.Requests.Notification
{
    using System;
    using System.Collections.Generic;
    using Core.Exporters;
    using Core.Facilities;
    using Core.Importer;
    using Core.Producers;
    using Core.Shared;

    public class OrganisationsInvolvedInfo
    {
        public Guid NotificationId { get; set; }

        public NotificationType NotificationType { get; set; }

        public bool IsExporterCompleted { get; set; }

        public bool IsProducerCompleted { get; set; }

        public bool HasSiteOfExport { get; set; }

        public bool IsImporterCompleted { get; set; }

        public bool IsFacilityCompleted { get; set; }

        public bool HasActualSiteOfTreatment { get; set; }

        public ExporterData Exporter { get; set; }

        public List<ProducerData> Producers { get; set; }

        public ImporterData Importer { get; set; }

        public List<FacilityData> Facilities { get; set; }
    }
}