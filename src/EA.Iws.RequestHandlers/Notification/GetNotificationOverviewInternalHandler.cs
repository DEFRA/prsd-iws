namespace EA.Iws.RequestHandlers.Notification
{
    using System;
    using System.Threading.Tasks;
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
        private readonly INotificationApplicationOverviewRepository overviewRepository;
        private readonly IMapper mapper;

        public GetNotificationOverviewInternalHandler(IwsContext context,
            IUserContext userContext,
            INotificationApplicationOverviewRepository overviewRepository,
            IMapper mapper)
        {
            this.context = context;
            this.userContext = userContext;
            this.mapper = mapper;
            this.overviewRepository = overviewRepository;
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
            
            var overviewData = await overviewRepository.GetById(message.NotificationId);

            return mapper.Map<NotificationOverview>(overviewData);
        }
    }
}