namespace EA.Iws.RequestHandlers.TechnologyEmployed
{
    using System.Threading.Tasks;
    using Core.TechnologyEmployed;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.TechnologyEmployed;

    internal class GetTechnologyEmployedHandler : IRequestHandler<GetTechnologyEmployed, TechnologyEmployedData>
    {
        private readonly INotificationApplicationRepository notificationRepository;
        private readonly IMap<NotificationApplication, TechnologyEmployedData> mapper;

        public GetTechnologyEmployedHandler(INotificationApplicationRepository notificationRepository,
            IMap<NotificationApplication, TechnologyEmployedData> mapper)
        {
            this.notificationRepository = notificationRepository;
            this.mapper = mapper;
        }

        public async Task<TechnologyEmployedData> HandleAsync(GetTechnologyEmployed message)
        {
            var notification = await notificationRepository.GetById(message.NotificationId);

            return mapper.Map(notification);
        }
    }
}