namespace EA.Iws.Requests.Admin.Search
{
    using System.Collections.Generic;
    using Core.Admin.Search;
    using Prsd.Core.Mediator;

    public class SearchExportNotifications : IRequest<IList<BasicSearchResult>>
    {
        public string SearchTerm { get; set; }

        public SearchExportNotifications(string searchTerm)
        {
            SearchTerm = searchTerm;
        }
    }
}