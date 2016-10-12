namespace EA.Iws.Domain.Search
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Admin.Search;
    using Core.Notification;
    using ImportNotification;

    public interface IAdvancedSearchRepository
    {
        Task<IEnumerable<BasicSearchResult>> SearchExportNotificationsByCriteria(AdvancedSearchCriteria criteria, UKCompetentAuthority competentAuthority);

        Task<IEnumerable<ImportNotificationSearchResult>> SearchImportNotificationsByCriteria(AdvancedSearchCriteria criteria, UKCompetentAuthority competentAuthority);
    }
}