namespace EA.Iws.RequestHandlers.Notification
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Notification;
    using Core.Notification.Overview;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Domain;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GetNotificationOverviewInternalHandler : IRequestHandler<GetNotificationOverviewInternal, NotificationOverview>
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;
        private readonly INotificationProgressService progressService;
        private readonly IMapper mapper;

        public GetNotificationOverviewInternalHandler(IwsContext context,
            IUserContext userContext,
            INotificationProgressService progressService,
            IMapper mapper)
        {
            this.context = context;
            this.userContext = userContext;
            this.progressService = progressService;
            this.mapper = mapper;
        }

        public async Task<NotificationOverview> HandleAsync(GetNotificationOverviewInternal message)
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

            var overviewData = await overviewDataQuery.SingleAsync();

            return new NotificationOverview
            {
                NotificationType = (Core.Shared.NotificationType)overviewData.Notification.NotificationType.Value,
                Progress = progressService.GetNotificationProgressInfo(message.NotificationId),
                ShipmentOverview = mapper.Map<ShipmentOverview>(overviewData.Notification),
                WasteClassificationOverview = mapper.Map<WasteClassificationOverview>(overviewData.Notification),
                CompetentAuthority = (CompetentAuthority)overviewData.Notification.CompetentAuthority.Value,
                WasteRecovery = mapper.Map<WasteRecoveryOverview>(overviewData.WasteRecovery),
                WasteDisposal = mapper.Map<WasteDisposalOverview>(overviewData.WasteDisposal),
                NotificationNumber = overviewData.Notification.NotificationNumber,
                NotificationId = overviewData.Notification.Id,
                Journey = mapper.Map<Journey>(overviewData.Notification),
                RecoveryOperation = mapper.Map<RecoveryOperation>(overviewData.Notification),
                OrganisationsInvolved = mapper.Map<OrganisationsInvolved>(overviewData.Notification),
                Transportation = mapper.Map<Transportation>(overviewData.Notification),
                WasteCodesOverview = mapper.Map<WasteCodesOverviewInfo>(overviewData.Notification),
                SubmitSummaryData = mapper.Map<SubmitSummaryData>(overviewData.Notification)
            };
        }
    }
}