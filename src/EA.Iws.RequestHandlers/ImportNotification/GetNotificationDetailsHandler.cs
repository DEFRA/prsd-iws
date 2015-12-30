namespace EA.Iws.RequestHandlers.ImportNotification
{
    using System.Threading.Tasks;
    using Core.ImportNotification;
    using Domain.ImportNotification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;

    internal class GetNotificationDetailsHandler : IRequestHandler<GetNotificationDetails, NotificationDetails>
    {
        private readonly IMapper mapper;
        private readonly IImportNotificationRepository repository;

        public GetNotificationDetailsHandler(IImportNotificationRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<NotificationDetails> HandleAsync(GetNotificationDetails message)
        {
            var notification = await repository.Get(message.ImportNotificationId);
            return mapper.Map<NotificationDetails>(notification);
        }
    }
}