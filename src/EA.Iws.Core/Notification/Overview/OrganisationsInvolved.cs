namespace EA.Iws.Core.Notification.Overview
{
    using System;
    using System.Collections.Generic;
    using Exporters;
    using Facilities;
    using Importer;
    using Producers;
    using Shared;

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