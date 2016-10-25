namespace EA.Iws.RequestHandlers.Mappings.ImportNotification
{
    using Core.Admin.Search;
    using Domain.ImportNotification;
    using Prsd.Core.Mapper;

    internal class SearchResultMap : IMap<ImportNotificationSearchResult, ImportSearchResult>
    {
        public ImportSearchResult Map(ImportNotificationSearchResult source)
        {
            return new ImportSearchResult
            {
                Id = source.NotificationId,
                NotificationType = source.NotificationType,
                NotificationNumber = source.Number,
                Status = source.Status
            };
        }
    }
}
