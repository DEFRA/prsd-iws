namespace EA.Iws.Requests.Admin.Search
{
    using System.Collections.Generic;
    using Core.Admin.Search;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(GeneralPermissions.CanViewSearchResults)]
    public class SearchExportNotifications : IRequest<IList<BasicSearchResult>>
    {
        public string SearchTerm { get; set; }

        public SearchExportNotifications(string searchTerm)
        {
            SearchTerm = searchTerm;
        }
    }
}