namespace EA.Iws.Core.Admin.Search
{
    using System;
    using Shared;

    public class ImportSearchResult
    {
        public Guid Id { get; set; }

        public string NotificationNumber { get; set; }

        public NotificationType NotificationType { get; set; }
    }
}
