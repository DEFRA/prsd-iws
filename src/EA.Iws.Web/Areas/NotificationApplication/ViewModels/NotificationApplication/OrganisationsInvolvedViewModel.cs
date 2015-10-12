namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using Core.Exporters;
    using Core.Facilities;
    using Core.Importer;
    using Core.Notification;
    using Core.Notification.Overview;
    using Core.Producers;
    using Core.Shared;

    public class OrganisationsInvolvedViewModel
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
       
        public OrganisationsInvolvedViewModel() 
        {
        }

        public OrganisationsInvolvedViewModel(OrganisationsInvolved info, NotificationApplicationCompletionProgress progress)
        {
            NotificationId = info.NotificationId;
            NotificationType = info.NotificationType;
            IsExporterCompleted = progress.HasExporter;
            HasSiteOfExport = progress.HasSiteOfExport;
            IsProducerCompleted = progress.HasProducer;
            IsImporterCompleted = progress.HasImporter;
            IsFacilityCompleted = progress.HasFacility;
            HasActualSiteOfTreatment = progress.HasActualSiteOfTreatment;
            Exporter = info.Exporter;
            Producers = info.Producers;
            Importer = info.Importer;
            Facilities = info.Facilities;
        }
    }
}