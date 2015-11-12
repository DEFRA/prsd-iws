namespace EA.Iws.Requests.Admin.Search
{
    using System.Collections.Generic;
    using Core.Admin.Search;
    using Prsd.Core.Mediator;

    public class SearchImportNotifications : IRequest<IList<ImportSearchResult>>
    {
        public string NotificationNumber { get; private set; }

        public SearchImportNotifications(string notificationNumber)
        {
            NotificationNumber = notificationNumber;
        }
    }
}
