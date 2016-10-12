namespace EA.Iws.DataAccess.Repositories.Search
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Admin.Search;
    using Core.Notification;
    using Domain.ImportNotification;
    using Domain.Search;

    internal class AdvancedSearchRepository : IAdvancedSearchRepository
    {
        private const string SearchQuery = @"SELECT DISTINCT [Id]
                    FROM	[Search].[Notifications]
                    WHERE	[CompetentAuthority] = @ca
                    AND     [ImportOrExport] = @importOrExport
                    AND     (@ewc IS NULL OR ([CodeType] = 3 AND [Code] LIKE '%@ewc%'))
                    AND     (@producerName IS NULL OR [ProducerName] LIKE '%' + @producerName + '%')
                    AND     (@importerName IS NULL OR [ImporterName] LIKE '%' + @importerName + '%')
                    AND     (@importCountryName IS NULL OR [CountryOfImport] LIKE '%' + @importCountryName + '%')
                    AND     (@localAreaId IS NULL OR [LocalAreaId] = @localAreaId)
                    AND     (@consentValidFrom IS NULL OR [ConsentValidFrom] = @consentValidFrom)
                    AND     (@consentValidTo IS NULL OR [ConsentValidTo] = @consentValidTo)";

        private readonly IwsContext context;

        public AdvancedSearchRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<BasicSearchResult>> SearchExportNotificationsByCriteria(AdvancedSearchCriteria criteria, UKCompetentAuthority competentAuthority)
        {
            var result = await GetSearchResults(criteria, competentAuthority, "Export");

            var parameters = result.Select((x, i) => new SqlParameter("@id" + i, x)).ToArray();

            var queryFormat = @"
                SELECT
                    N.[Id],
                    N.[NotificationNumber],
                    NS.[Description] AS [NotificationStatus],
                    E.[Name] AS [ExporterName],
                    CCT.[Description] AS [WasteType]
                FROM
                    [Notification].[Notification] N
                    INNER JOIN [Notification].[NotificationAssessment] NA 
                        INNER JOIN [Lookup].[NotificationStatus] NS ON NA.[Status] = NS.Id
                    ON N.Id = NA.NotificationApplicationId
                    LEFT JOIN [Notification].[Exporter] E ON N.Id = E.NotificationId
                    LEFT JOIN [Notification].[WasteType] WT 
                        INNER JOIN [Lookup].[ChemicalCompositionType] CCT ON WT.ChemicalCompositionType = CCT.Id
                    ON N.Id = WT.NotificationId
                WHERE
                    N.[Id] IN ({0})";

            var query = string.Format(queryFormat, string.Join(",", parameters.Select(x => x.ParameterName)));

            return await context.Database.SqlQuery<BasicSearchResult>(query, parameters).ToListAsync();
        }

        public async Task<IEnumerable<ImportNotificationSearchResult>> SearchImportNotificationsByCriteria(AdvancedSearchCriteria criteria, UKCompetentAuthority competentAuthority)
        {
            var result = await GetSearchResults(criteria, competentAuthority, "Import");

            return new ImportNotificationSearchResult[] { };
        }

        private async Task<Guid[]> GetSearchResults(AdvancedSearchCriteria criteria, UKCompetentAuthority competentAuthority, string importOrExport)
        {
            return await context.Database.SqlQuery<Guid>(
                SearchQuery,
                new SqlParameter("@ca", (int)competentAuthority),
                new SqlParameter("@importOrExport", importOrExport),
                new SqlParameter("@ewc", (object)criteria.EwcCode ?? DBNull.Value),
                new SqlParameter("@producerName", (object)criteria.ProducerName ?? DBNull.Value),
                new SqlParameter("@importerName", (object)criteria.ImporterName ?? DBNull.Value),
                new SqlParameter("@importCountryName", (object)criteria.ImportCountryName ?? DBNull.Value),
                new SqlParameter("@localAreaId", (object)criteria.LocalAreaId ?? DBNull.Value),
                new SqlParameter("@consentValidFrom", (object)criteria.ConsentValidFrom ?? DBNull.Value),
                new SqlParameter("@consentValidTo", (object)criteria.ConsentValidTo ?? DBNull.Value)).ToArrayAsync();
        }
    }
}