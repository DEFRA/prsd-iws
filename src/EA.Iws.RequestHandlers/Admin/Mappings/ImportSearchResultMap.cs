namespace EA.Iws.RequestHandlers.Admin.Mappings
{
    using Core.Admin.Search;
    using Domain.ImportNotification;
    using Prsd.Core.Mapper;

    internal class ImportSearchResultMap : IMap<ImportNotification, ImportSearchResult>
    {
        public ImportSearchResult Map(ImportNotification source)
        {
            return new ImportSearchResult
            {
                Id = source.Id,
                NotificationNumber = source.NotificationNumber,
                NotificationType = source.NotificationType
            };
        }
    }
}
