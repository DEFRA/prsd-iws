namespace EA.Iws.RequestHandlers.Notification
{
    using System.Threading.Tasks;
    using Core.Notification.Overview;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GetNotificationOverviewHandler : IRequestHandler<GetNotificationOverview, NotificationOverview>
    {
        private readonly INotificationApplicationOverviewRepository overviewRepository;
        private readonly IMapper mapper;

        public GetNotificationOverviewHandler(
            INotificationApplicationOverviewRepository overviewRepository,
            IMapper mapper)
        {
            this.overviewRepository = overviewRepository;
            this.mapper = mapper;
        }

        public async Task<NotificationOverview> HandleAsync(GetNotificationOverview message)
        {
            var overviewData = await overviewRepository.GetById(message.NotificationId);
            
            return mapper.Map<NotificationOverview>(overviewData);
        }
    }
}