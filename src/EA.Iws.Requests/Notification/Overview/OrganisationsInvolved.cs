namespace EA.Iws.Requests.Notification.Overview
{
    using System;
    using System.Collections.Generic;
    using Core.Exporters;
    using Core.Facilities;
    using Core.Importer;
    using Core.Producers;
    using Core.Shared;

    public class OrganisationsInvolved
    {
        public Guid NotificationId { get; set; }

        public NotificationType NotificationType { get; set; }

        public ExporterData Exporter { get; set; }

        public List<ProducerData> Producers { get; set; }

        public ImporterData Importer { get; set; }

        public List<FacilityData> Facilities { get; set; }
    }
}