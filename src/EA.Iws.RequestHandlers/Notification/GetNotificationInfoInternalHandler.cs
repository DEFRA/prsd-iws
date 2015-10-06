namespace EA.Iws.RequestHandlers.Notification
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Notification;
    using DataAccess;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Shipment;
    using Domain.NotificationApplication.WasteRecovery;
    using Prsd.Core.Domain;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using Requests.Notification.Overview;

    internal class GetNotificationInfoInternalHandler : IRequestHandler<GetNotificationInfoInternal, NotificationOverview>
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;
        private readonly IMap<NotificationApplication, ShipmentOverview> shipmentMap;
        private readonly IMap<NotificationApplication, WasteClassificationOverview> wasteClassificationMap;
        private readonly IMap<NotificationApplication, OrganisationsInvolved> organisationsInvolvedInfoMap;
        private readonly IMap<NotificationApplication, RecoveryOperation> recoveryOperationInfoMap;
        private readonly IMap<NotificationApplication, Transportation> transportationInfoMap;
        private readonly IMap<NotificationApplication, Journey> journeyInfoMap;
        private readonly IMap<NotificationApplication, WasteCodesOverviewInfo> wasteCodesOverviewMap;
        private readonly IMap<WasteRecovery, WasteRecoveryOverview> wasteRecoveryMap;
        private readonly IMap<WasteDisposal, WasteDisposalOverview> wasteDisposalMap;
        private readonly INotificationProgressService progressService;
        private readonly IMap<NotificationApplication, SubmitSummaryData> submitSummaryDataMap;

        public GetNotificationInfoInternalHandler(IwsContext context,
            IUserContext userContext,
            INotificationProgressService progressService,
            IShipmentInfoRepository shipmentInfoRepository,
            IMap<NotificationApplication, ShipmentOverview> shipmentMap,
            IMap<NotificationApplication, WasteClassificationOverview> wasteClassificationMap,
            IMap<NotificationApplication, OrganisationsInvolved> organisationsInvolvedInfoMap,
            IMap<NotificationApplication, RecoveryOperation> recoveryOperationInfoMap,
            IMap<NotificationApplication, Transportation> transportationInfoMap,
            IMap<NotificationApplication, Journey> journeyInfoMap,
            IMap<NotificationApplication, WasteCodesOverviewInfo> asteCodesOverviewMap,
            IMap<NotificationApplication, SubmitSummaryData> submitSummaryDataMap,
            IMap<NotificationApplication, WasteCodesOverviewInfo> wasteCodesOverviewMap,
            IMap<WasteRecovery, WasteRecoveryOverview> wasteRecoveryMap,
            IMap<WasteDisposal, WasteDisposalOverview> wasteDisposalMap)
        {
            this.submitSummaryDataMap = submitSummaryDataMap;
            this.progressService = progressService;
            this.wasteDisposalMap = wasteDisposalMap;
            this.wasteRecoveryMap = wasteRecoveryMap;
            this.wasteCodesOverviewMap = wasteCodesOverviewMap;
            this.journeyInfoMap = journeyInfoMap;
            this.transportationInfoMap = transportationInfoMap;
            this.recoveryOperationInfoMap = recoveryOperationInfoMap;
            this.organisationsInvolvedInfoMap = organisationsInvolvedInfoMap;
            this.wasteClassificationMap = wasteClassificationMap;
            this.shipmentMap = shipmentMap;
            this.context = context;
            this.userContext = userContext;
        }

        public async Task<NotificationOverview> HandleAsync(GetNotificationInfoInternal message)
        {
            if (!await context.IsInternalUserAsync(userContext))
            {
                throw new InvalidOperationException(
                    string.Format("Cannot access the notification {0} because the requesting user {1} is not an internal user.",
                    message.NotificationId,
                    userContext.UserId));
            }

            //TODO: this should be turned into some sort of repository, it's untidy and is being used in two places!

            var overviewDataQuery =
                from notification in context.NotificationApplications
                where notification.Id == message.NotificationId
                from wasteRecovery
                //left join waste recovery, if it exists
                in context.WasteRecoveries
                    .Where(wr => wr.NotificationId == notification.Id)
                    .DefaultIfEmpty()
                from assessment
                //left join assessment, if it exists
                in context.NotificationAssessments
                    .Where(na => na.NotificationApplicationId == notification.Id)
                    .DefaultIfEmpty()
                from wasteDiposal
                in context.WasteDisposals
                    .Where(wd => wd.NotificationId == notification.Id)
                    .DefaultIfEmpty()
                select new
                {
                    Notification = notification,
                    WasteRecovery = wasteRecovery,
                    WasteDisposal = wasteDiposal,
                    NotificationAssessment = assessment
                };

            var overviewData = await overviewDataQuery.SingleOrDefaultAsync();

            return new NotificationOverview
            {
                NotificationType = (Core.Shared.NotificationType)overviewData.Notification.NotificationType.Value,
                Progress = progressService.GetNotificationProgressInfo(message.NotificationId),
                ShipmentOverview = shipmentMap.Map(overviewData.Notification),
                WasteClassificationOverview = wasteClassificationMap.Map(overviewData.Notification),
                CompetentAuthority = (CompetentAuthority)overviewData.Notification.CompetentAuthority.Value,
                WasteRecovery = wasteRecoveryMap.Map(overviewData.WasteRecovery),
                WasteDisposal = wasteDisposalMap.Map(overviewData.WasteDisposal),
                NotificationNumber = overviewData.Notification.NotificationNumber,
                NotificationId = overviewData.Notification.Id,
                Journey = journeyInfoMap.Map(overviewData.Notification),
                RecoveryOperation = recoveryOperationInfoMap.Map(overviewData.Notification),
                OrganisationsInvolved = organisationsInvolvedInfoMap.Map(overviewData.Notification),
                Transportation = transportationInfoMap.Map(overviewData.Notification),
                WasteCodesOverview = wasteCodesOverviewMap.Map(overviewData.Notification),
                SubmitSummaryData = submitSummaryDataMap.Map(overviewData.Notification)
            };
        }
    }
}