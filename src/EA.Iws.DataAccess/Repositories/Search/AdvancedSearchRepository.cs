namespace EA.Iws.DataAccess.Repositories.Search
{
    using Core.Admin.Search;
    using Core.Notification;
    using Core.OperationCodes;
    using Core.Shared;
    using Domain.Search;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;

    internal class AdvancedSearchRepository : IAdvancedSearchRepository
    {
        private readonly IwsContext context;

        public AdvancedSearchRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<ExportAdvancedSearchResult>> SearchExportNotificationsByCriteria(AdvancedSearchCriteria criteria,
                                                                                                       UKCompetentAuthority competentAuthority)
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

            var exportNotificationIds = string.Join(",", result);

            var sqlQuery = @"EXEC [Notification].[uspGetExportNotifications] @NotificationIds";

            var parameter = new SqlParameter("@NotificationIds", exportNotificationIds);

            var results = await context.Database
                                       .SqlQuery<ExportAdvancedSearchResult>(sqlQuery, parameter)
                                       .ToListAsync();

            return results;
        }

        public async Task<IEnumerable<ImportAdvancedSearchResult>> SearchImportNotificationsByCriteria(AdvancedSearchCriteria criteria,
                                                                                                       UKCompetentAuthority competentAuthority)
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

            var exportNotificationIds = string.Join(",", result);

            var sqlQuery = @"EXEC [ImportNotification].[uspGetImportNotifications] @NotificationIds";

            var parameter = new SqlParameter("@NotificationIds", exportNotificationIds);

            var results = await context.Database
                                       .SqlQuery<ImportAdvancedSearchResult>(sqlQuery, parameter)
                                       .ToListAsync();

            return results;
        }

        private async Task<Guid[]> GetSearchResults(AdvancedSearchCriteria criteria, UKCompetentAuthority competentAuthority, string importOrExport)
        {
            var sqlQuery = @"EXEC [Search].[uspGetNotifications]
                                    @ca = @ca,
                                    @importOrExport = @importOrExport,
                                    @ewc = @ewc,
                                    @baselOecd = @baselOecd,
                                    @producerName = @producerName,
                                    @importerName = @importerName,
                                    @exporterName = @exporterName,
                                    @facilityName = @facilityName,
                                    @importCountryName = @importCountryName,
                                    @exitPointName = @exitPointName,
                                    @entryPointName = @entryPointName,
                                    @localAreaId = @localAreaId,
                                    @consentValidFromStart = @consentValidFromStart,
                                    @consentValidFromEnd = @consentValidFromEnd,
                                    @consentValidToStart = @consentValidToStart,
                                    @consentValidToEnd = @consentValidToEnd,
                                    @notificationReceivedStart = @notificationReceivedStart,
                                    @notificationReceivedEnd = @notificationReceivedEnd,
                                    @notificationType = @notificationType,
                                    @exportStatus = @exportStatus,
                                    @importStatus = @importStatus,
                                    @isInterim = @isInterim,
                                    @exportCountryName = @exportCountryName,
                                    @operationCodes = @operationCodes,
                                    @baselOecdCodeNotListed = @baselOecdCodeNotListed
                            ";

            var parameters = new[]
            {
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
                new SqlParameter("@notificationReceivedStart", (object)criteria.NotificationReceivedStart ?? DBNull.Value),
                new SqlParameter("@notificationReceivedEnd", (object)criteria.NotificationReceivedEnd ?? DBNull.Value),
                new SqlParameter("@notificationType", (object)criteria.NotificationType ?? DBNull.Value),
                new SqlParameter("@exportStatus", (object)criteria.NotificationStatus ?? DBNull.Value),
                new SqlParameter("@importStatus", (object)criteria.ImportNotificationStatus ?? DBNull.Value),
                new SqlParameter("@isInterim", (object)criteria.IsInterim ?? DBNull.Value),
                new SqlParameter("@exportCountryName", (object)criteria.ExportCountryName ?? DBNull.Value),
                new SqlParameter("@operationCodes", GetOperationCodes(criteria.OperationCodes)),
                new SqlParameter("@baselOecdCodeNotListed", (object)criteria.BaselOecdCodeNotListed ?? DBNull.Value)
            };

            var result = await context.Database
                                      .SqlQuery<Guid>(sqlQuery, parameters)
                                      .ToArrayAsync();

            return result;
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