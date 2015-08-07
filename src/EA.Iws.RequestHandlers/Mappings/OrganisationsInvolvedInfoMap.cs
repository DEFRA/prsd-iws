namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core.Exporters;
    using Core.Facilities;
    using Core.Importer;
    using Core.Notification;
    using Core.Producers;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Requests.Notification;

    internal class OrganisationsInvolvedInfoMap : IMap<NotificationApplication, OrganisationsInvolvedInfo>
    {
        private readonly IMap<NotificationApplication, ExporterData> exporterMap;
        private readonly IMap<NotificationApplication, ImporterData> importerMap;
        private readonly IMap<NotificationApplication, IList<ProducerData>> producerMap;
        private readonly IMap<NotificationApplication, IList<FacilityData>> facilityMap;

        public OrganisationsInvolvedInfoMap(
            IMap<NotificationApplication, ExporterData> exporterMap,
            IMap<NotificationApplication, ImporterData> importerMap,
            IMap<NotificationApplication, IList<ProducerData>> producerMap,
            IMap<NotificationApplication, IList<FacilityData>> facilityMap)
        {
            this.exporterMap = exporterMap;
            this.importerMap = importerMap;
            this.producerMap = producerMap;
            this.facilityMap = facilityMap;
        }

        public OrganisationsInvolvedInfo Map(NotificationApplication notification)
        {
            return new OrganisationsInvolvedInfo
            {
                NotificationId = notification.Id,
                NotificationType = notification.NotificationType == NotificationType.Disposal
                        ? Core.Shared.NotificationType.Disposal
                        : Core.Shared.NotificationType.Recovery,
                Exporter = exporterMap.Map(notification),
                Importer = importerMap.Map(notification),
                Producers = producerMap.Map(notification).ToList(),
                Facilities = facilityMap.Map(notification).ToList()
            };
        }
    }
}
