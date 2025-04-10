﻿namespace EA.Iws.Requests.Admin.Search
{
    using System.Collections.Generic;
    using Core.Admin.Search;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(GeneralPermissions.CanViewSearchResults)]
    public class SearchImportNotifications : IRequest<IList<ImportSearchResult>>
    {
        public string SearchTerm { get; private set; }

        public SearchImportNotifications(string searchTerm)
        {
            SearchTerm = searchTerm;
        }
    }
}
