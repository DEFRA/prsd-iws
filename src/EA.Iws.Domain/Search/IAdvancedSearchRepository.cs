namespace EA.Iws.Domain.Search
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Admin.Search;
    using Core.Notification;

    public interface IAdvancedSearchRepository
    {
        Task<IEnumerable<ExportAdvancedSearchResult>> SearchExportNotificationsByCriteria(AdvancedSearchCriteria criteria, UKCompetentAuthority competentAuthority);

        Task<IEnumerable<ImportAdvancedSearchResult>> SearchImportNotificationsByCriteria(AdvancedSearchCriteria criteria, UKCompetentAuthority competentAuthority);
    }
}