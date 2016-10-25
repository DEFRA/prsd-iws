namespace EA.Iws.RequestHandlers.Admin.Search
{
    using System.Threading.Tasks;
    using Core.Admin.Search;
    using Domain;
    using Domain.Search;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.Admin.Search;

    internal class NotificaitonsAdvancedSearchHandler :
        IRequestHandler<NotificaitonsAdvancedSearch, AdvancedSearchResult>
    {
        private readonly IInternalUserRepository internalUserRepository;
        private readonly IAdvancedSearchRepository repository;
        private readonly IUserContext userContext;

        public NotificaitonsAdvancedSearchHandler(IAdvancedSearchRepository repository,
            IUserContext userContext, IInternalUserRepository internalUserRepository)
        {
            this.repository = repository;
            this.userContext = userContext;
            this.internalUserRepository = internalUserRepository;
        }

        public async Task<AdvancedSearchResult> HandleAsync(NotificaitonsAdvancedSearch message)
        {
            var user = await internalUserRepository.GetByUserId(userContext.UserId);
            var exportResults =
                await repository.SearchExportNotificationsByCriteria(message.Criteria, user.CompetentAuthority);
            var importResults =
                await repository.SearchImportNotificationsByCriteria(message.Criteria, user.CompetentAuthority);

            return new AdvancedSearchResult
            {
                ExportResults = exportResults,
                ImportResults = importResults
            };
        }
    }
}