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
        private readonly IImportNotificationSearchRepository importNotificationSearchRepository;
        private readonly IMapper mapper;

        public SearchImportNotificationsHandler(IImportNotificationSearchRepository importNotificationSearchRepository, IMapper mapper)
        {
            this.importNotificationSearchRepository = importNotificationSearchRepository;
            this.mapper = mapper;
        }

        public async Task<IList<ImportSearchResult>> HandleAsync(SearchImportNotifications message)
        {
            var results = await importNotificationSearchRepository.SearchByNumber(message.NotificationNumber);

            return results.Select(n => mapper.Map<ImportSearchResult>(n)).ToArray();
        }
    }
}
