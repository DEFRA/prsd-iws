namespace EA.Iws.RequestHandlers.Admin.Search
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Admin.Search;
    using Domain.ImportNotification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Admin.Search;

    internal class SearchImportNotificationsHandler : IRequestHandler<SearchImportNotifications, IList<ImportSearchResult>>
    {
        private readonly IImportNotificationRepository importNotificationRepository;
        private readonly IMapper mapper;

        public SearchImportNotificationsHandler(IImportNotificationRepository importNotificationRepository, IMapper mapper)
        {
            this.importNotificationRepository = importNotificationRepository;
            this.mapper = mapper;
        }

        public async Task<IList<ImportSearchResult>> HandleAsync(SearchImportNotifications message)
        {
            var notifications = await importNotificationRepository.SearchByNumber(message.NotificationNumber);

            return notifications.Select(n => mapper.Map<ImportSearchResult>(n)).ToArray();
        }
    }
}
