namespace EA.Iws.RequestHandlers.Notification
{
    using System.Threading.Tasks;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GetNotificationBasicInfoHandler : IRequestHandler<GetNotificationBasicInfo, NotificationBasicInfo>
    {
        private readonly IMap<NotificationApplication, NotificationBasicInfo> mapper;
        private readonly INotificationApplicationRepository repository;

        public GetNotificationBasicInfoHandler(INotificationApplicationRepository repository,
            IMap<NotificationApplication, NotificationBasicInfo> mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<NotificationBasicInfo> HandleAsync(GetNotificationBasicInfo message)
        {
            var notification = await repository.GetById(message.NotificationId);

            return mapper.Map(notification);
        }
    }
}