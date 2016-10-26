namespace EA.Iws.Requests.Admin.Search
{
    using Core.Admin.Search;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(GeneralPermissions.CanViewSearchResults)]
    public class NotificaitonsAdvancedSearch : IRequest<AdvancedSearchResult>
    {
        public NotificaitonsAdvancedSearch(AdvancedSearchCriteria criteria)
        {
            Criteria = criteria;
        }

        public AdvancedSearchCriteria Criteria { get; private set; }
    }
}