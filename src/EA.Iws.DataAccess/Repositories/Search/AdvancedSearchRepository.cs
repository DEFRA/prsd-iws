namespace EA.Iws.DataAccess.Repositories.Search
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Admin.Search;
    using Core.Notification;
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

        public async Task<IEnumerable<ExportAdvancedSearchResult>> SearchExportNotificationsByCriteria(AdvancedSearchCriteria criteria, UKCompetentAuthority competentAuthority)
        {
            var result = await GetSearchResults(criteria, competentAuthority, "Export");

            if (!result.Any())
            {
                return Enumerable.Empty<ExportAdvancedSearchResult>();
            }

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

            return await context.Database.SqlQuery<ExportAdvancedSearchResult>(query, parameters).ToListAsync();
        }

        public async Task<IEnumerable<ImportAdvancedSearchResult>> SearchImportNotificationsByCriteria(AdvancedSearchCriteria criteria, UKCompetentAuthority competentAuthority)
        {
            var result = await GetSearchResults(criteria, competentAuthority, "Import");

            if (!result.Any())
            {
                return Enumerable.Empty<ImportAdvancedSearchResult>();
            }

            var parameters = result.Select((x, i) => new SqlParameter("@id" + i, x)).ToArray();

            var queryFormat = @"
                SELECT
                    N.[Id],
                    N.[NotificationNumber],
                    S.[Description] AS [Status],
                    E.Name AS [Exporter],
                    CASE WHEN WT.BaselOecdCodeNotListed = 1 THEN 'Not listed' ELSE WC.Code END AS [BaselOecdCode]
                FROM
                    [ImportNotification].[Notification] N
                    INNER JOIN [ImportNotification].[NotificationAssessment] NA ON N.Id = NA.NotificationApplicationId
                    INNER JOIN [Lookup].[ImportNotificationStatus] S ON NA.[Status] = S.Id
                    INNER JOIN [ImportNotification].[Exporter] E ON N.Id = E.ImportNotificationId
                    INNER JOIN [ImportNotification].[WasteType] WT ON N.Id = WT.ImportNotificationId
                    LEFT JOIN [ImportNotification].[WasteCode] W 
                        INNER JOIN [Lookup].[WasteCode] WC ON W.WasteCodeId = WC.Id
                        ON WT.Id = W.WasteTypeId AND WC.CodeType IN (1, 2)
                WHERE
                    N.[Id] IN ({0})";

            var query = string.Format(queryFormat, string.Join(",", parameters.Select(x => x.ParameterName)));

            return await context.Database.SqlQuery<ImportAdvancedSearchResult>(query, parameters).ToListAsync();
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