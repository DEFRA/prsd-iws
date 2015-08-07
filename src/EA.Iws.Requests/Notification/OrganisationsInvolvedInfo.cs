namespace EA.Iws.Requests.Notification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using EA.Iws.Core.Exporters;
    using EA.Iws.Core.Facilities;
    using EA.Iws.Core.Importer;
    using EA.Iws.Core.Notification;
    using EA.Iws.Core.Producers;
    using EA.Iws.Core.Shared;
    
    public class OrganisationsInvolvedInfo
    {
        public Guid NotificationId { get; set; }
        public NotificationType NotificationType { get; set; }
        public bool IsExporterCompleted { get; set; }
        public bool IsProducerCompleted { get; set; }
        public bool IsImporterCompleted { get; set; }
        public bool IsFacilityCompleted { get; set; }
        public ExporterData Exporter { get; set; }
        public List<ProducerData> Producers { get; set; }
        public ImporterData Importer { get; set; }
        public List<FacilityData> Facilities { get; set; }
    }
}
