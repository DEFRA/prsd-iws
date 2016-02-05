namespace EA.Iws.RequestHandlers.Mappings
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Exporters;
    using Core.Facilities;
    using Core.Importer;
    using Core.Notification;
    using Core.Notification.Overview;
    using Core.Producers;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;

    internal class NotificationOverviewMap : IMap<NotificationApplicationOverview, NotificationOverview>
    {
        private readonly IMapper mapper;

        public NotificationOverviewMap(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public NotificationOverview Map(NotificationApplicationOverview source)
        {
            return new NotificationOverview
            {
                NotificationId = source.Notification.Id,
                NotificationType = source.Notification.NotificationType,
                NotificationNumber = source.Notification.NotificationNumber,
                CompetentAuthority = source.Notification.CompetentAuthority,
                Progress = source.Progress,
                ShipmentOverview = mapper.Map<ShipmentOverview>(source.Notification),
                WasteClassificationOverview = mapper.Map<WasteClassificationOverview>(source.Notification),
                WasteRecovery = mapper.Map<WasteRecoveryOverview>(source.WasteRecovery),
                WasteDisposal = mapper.Map<WasteDisposalOverview>(source.WasteDisposal),
                Journey = mapper.Map<Journey>(source.Notification),
                RecoveryOperation = mapper.Map<RecoveryOperation>(source.Notification),
                OrganisationsInvolved = MapOrganisationsInvolved(source),
                Transportation = mapper.Map<Transportation>(source.Notification),
                SubmitSummaryData = mapper.Map<SubmitSummaryData>(source.Notification),
                WasteCodesOverview = mapper.Map<WasteCodesOverviewInfo>(source.Notification),
                CanEditNotification = source.NotificationAssessment.CanEditNotification,
                NotificationCharge = source.Charge
            };
        }

        private OrganisationsInvolved MapOrganisationsInvolved(NotificationApplicationOverview source)
        {
            var organisationsInvolved = new OrganisationsInvolved
            {
                NotificationId = source.Notification.Id,
                NotificationType = source.Notification.NotificationType,
                Exporter = mapper.Map<ExporterData>(source.Exporter),
                Importer = mapper.Map<ImporterData>(source.Importer),
                Facilities = mapper.Map<IList<FacilityData>>(source.Notification).ToList(),
                Producers = mapper.Map<IList<ProducerData>>(source.Notification).ToList()
            };

            return organisationsInvolved;
        }
    }
}
