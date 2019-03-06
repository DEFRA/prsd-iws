namespace EA.Iws.DataAccess.Repositories.Search
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Admin.Search;
    using Core.Notification;
    using Core.OperationCodes;
    using Core.Shared;
    using Domain.Search;

    internal class AdvancedSearchRepository : IAdvancedSearchRepository
    {
        private const string SearchQuery = @"SELECT DISTINCT [Id]
                    FROM    [Search].[Notifications]
                    WHERE   [CompetentAuthority] = @ca
                    AND     [ImportOrExport] = @importOrExport
                    AND     (@ewc IS NULL OR ([EwcCode] LIKE '%' + @ewc + '%'))
                    AND     (@baselOecd IS NULL OR ([BaselOecdCode] LIKE '%' + @baselOecd + '%'))
                    AND     (@producerName IS NULL OR [ProducerName] LIKE '%' + @producerName + '%')
                    AND     (@importerName IS NULL OR [ImporterName] LIKE '%' + @importerName + '%')
                    AND     (@exporterName IS NULL OR [ExporterName] LIKE '%' + @exporterName + '%')
                    AND     (@facilityName IS NULL OR [FacilityName] LIKE '%' + @facilityName + '%')
                    AND     (@importCountryName IS NULL OR [CountryOfImport] LIKE '%' + @importCountryName + '%')
                    AND     (@exitPointName IS NULL OR [ExitPointName] LIKE '%' + @exitPointName + '%')
                    AND     (@entryPointName IS NULL OR [EntryPointName] LIKE '%' + @entryPointName + '%')
                    AND     (@localAreaId IS NULL OR [LocalAreaId] = @localAreaId)
                    AND     (@consentValidFromStart IS NULL OR [ConsentValidFrom] BETWEEN @consentValidFromStart AND COALESCE(@consentValidFromEnd, GETDATE()))
                    AND     (@consentValidToStart IS NULL OR [ConsentValidTo] BETWEEN @consentValidToStart AND COALESCE(@consentValidToEnd, GETDATE()))
                    AND     (@notificationReceivedStart IS NULL OR [NotificationReceivedDate] BETWEEN @notificationReceivedStart AND COALESCE(@notificationReceivedEnd, GETDATE()))
                    AND     (@notificationType IS NULL OR [NotificationType] = @notificationType)
                    AND     (@exportStatus IS NULL OR [ExportStatus] = @exportStatus)
                    AND     (@importStatus IS NULL OR [ImportStatus] = @importStatus)
                    AND     (@isInterim IS NULL OR [IsInterim] = @isInterim)
                    AND     (@exportCountryName IS NULL OR [CountryOfExport] LIKE '%' + @exportCountryName + '%')
                    AND     (@operationCodes IS NULL OR [OperationCodes] = @operationCodes)
                    AND     (@baselOecdCodeNotListed IS NULL OR [BaselOecdCodeNotListed] = @baselOecdCodeNotListed)";

        private readonly IwsContext context;

        public AdvancedSearchRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<ExportAdvancedSearchResult>> SearchExportNotificationsByCriteria(AdvancedSearchCriteria criteria, UKCompetentAuthority competentAuthority)
        {
            if (criteria.TradeDirection == TradeDirection.Import)
            {
                return Enumerable.Empty<ExportAdvancedSearchResult>();
            }

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
                    CCT.[Description] AS [WasteType],
					CASE WHEN NS.[Description] IN ('Consented', 'Consent withdrawn') THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS [ShowShipmentSummaryLink]
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
            if (criteria.TradeDirection == TradeDirection.Export)
            {
                return Enumerable.Empty<ImportAdvancedSearchResult>();
            }

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
                    CASE WHEN WT.BaselOecdCodeNotListed = 1 THEN 'Not listed' ELSE WC.Code END AS [BaselOecdCode],
					CASE WHEN S.[Description] IN ('Consented', 'Consent withdrawn') THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS [ShowShipmentSummaryLink]
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
            return await context.Database.SqlQuery<Guid>(SearchQuery,
                new SqlParameter("@ca", (int)competentAuthority),
                new SqlParameter("@importOrExport", importOrExport),
                new SqlParameter("@ewc", (object)criteria.EwcCode ?? DBNull.Value),
                new SqlParameter("@baselOecd", (object)criteria.BaselOecdCode ?? DBNull.Value),
                new SqlParameter("@producerName", (object)criteria.ProducerName ?? DBNull.Value),
                new SqlParameter("@importerName", (object)criteria.ImporterName ?? DBNull.Value),
                new SqlParameter("@exporterName", (object)criteria.ExporterName ?? DBNull.Value),
                new SqlParameter("@facilityName", (object)criteria.FacilityName ?? DBNull.Value),
                new SqlParameter("@importCountryName", (object)criteria.ImportCountryName ?? DBNull.Value),
                new SqlParameter("@exitPointName", (object)criteria.ExitPointName ?? DBNull.Value),
                new SqlParameter("@entryPointName", (object)criteria.EntryPointName ?? DBNull.Value),
                new SqlParameter("@localAreaId", (object)criteria.LocalAreaId ?? DBNull.Value),
                new SqlParameter("@consentValidFromStart", (object)criteria.ConsentValidFromStart ?? DBNull.Value),
                new SqlParameter("@consentValidFromEnd", (object)criteria.ConsentValidFromEnd ?? DBNull.Value),
                new SqlParameter("@consentValidToStart", (object)criteria.ConsentValidToStart ?? DBNull.Value),
                new SqlParameter("@consentValidToEnd", (object)criteria.ConsentValidToEnd ?? DBNull.Value),
                new SqlParameter("@notificationReceivedStart",
                    (object)criteria.NotificationReceivedStart ?? DBNull.Value),
                new SqlParameter("@notificationReceivedEnd", (object)criteria.NotificationReceivedEnd ?? DBNull.Value),
                new SqlParameter("@notificationType", (object)criteria.NotificationType ?? DBNull.Value),
                new SqlParameter("@exportStatus", (object)criteria.NotificationStatus ?? DBNull.Value),
                new SqlParameter("@importStatus", (object)criteria.ImportNotificationStatus ?? DBNull.Value),
                new SqlParameter("@isInterim", (object)criteria.IsInterim ?? DBNull.Value),
                new SqlParameter("@exportCountryName", (object)criteria.ExportCountryName ?? DBNull.Value),
                new SqlParameter("@operationCodes", GetOperationCodes(criteria.OperationCodes)),
                new SqlParameter("@baselOecdCodeNotListed", (object)criteria.BaselOecdCodeNotListed ?? DBNull.Value)).ToArrayAsync();
        }

        private static object GetOperationCodes(OperationCode[] operationCodes)
        {
            if (operationCodes == null || !operationCodes.Any())
            {
                return DBNull.Value;
            }

            return string.Join(",", operationCodes.OrderBy(x => x).Select(x => (int)x));
        }
    }
}