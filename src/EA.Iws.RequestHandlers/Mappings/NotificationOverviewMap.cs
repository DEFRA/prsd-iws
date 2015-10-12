namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.Notification;
    using Core.Notification.Overview;
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
                NotificationType = (Core.Shared.NotificationType)source.Notification.NotificationType.Value,
                NotificationNumber = source.Notification.NotificationNumber,
                CompetentAuthority = (CompetentAuthority)source.Notification.CompetentAuthority.Value,
                Progress = source.Progress,
                ShipmentOverview = mapper.Map<ShipmentOverview>(source.Notification),
                WasteClassificationOverview = mapper.Map<WasteClassificationOverview>(source.Notification),
                WasteRecovery = mapper.Map<Domain.NotificationApplication.WasteRecovery.WasteRecovery, WasteRecoveryOverview>(source.WasteRecovery),
                WasteDisposal = mapper.Map<Domain.NotificationApplication.WasteRecovery.WasteDisposal, WasteDisposalOverview>(source.WasteDisposal),
                Journey = mapper.Map<Journey>(source.Notification),
                RecoveryOperation = mapper.Map<RecoveryOperation>(source.Notification),
                OrganisationsInvolved = mapper.Map<OrganisationsInvolved>(source.Notification),
                Transportation = mapper.Map<Transportation>(source.Notification),
                SubmitSummaryData = mapper.Map<SubmitSummaryData>(source.Notification),
                WasteCodesOverview = mapper.Map<WasteCodesOverviewInfo>(source.Notification),
                CanEditNotification = source.NotificationAssessment.CanEditNotification,
                NotificationCharge = source.Charge
            };
        }
    }
}
