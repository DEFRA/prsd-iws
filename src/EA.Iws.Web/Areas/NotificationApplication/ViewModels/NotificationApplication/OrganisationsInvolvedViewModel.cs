namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Core.Exporters;
    using Core.Facilities;
    using Core.Importer;
    using Core.Notification;
    using Core.Producers;
    using Core.Shared;
    using Requests.Notification;
    
    public class OrganisationsInvolvedViewModel
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
       
        public OrganisationsInvolvedViewModel() 
        {
        }

        public OrganisationsInvolvedViewModel(OrganisationsInvolvedInfo info)
        {
            NotificationId = info.NotificationId;
            NotificationType = info.NotificationType;
            IsExporterCompleted = info.IsExporterCompleted;
            IsProducerCompleted = info.IsProducerCompleted;
            IsImporterCompleted = info.IsImporterCompleted;
            IsFacilityCompleted = info.IsFacilityCompleted;
            Exporter = info.Exporter;
            Producers = info.Producers;
            Importer = info.Importer;
            Facilities = info.Facilities;
        }
    }
}