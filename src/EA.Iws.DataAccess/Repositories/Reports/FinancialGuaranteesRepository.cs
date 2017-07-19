namespace EA.Iws.DataAccess.Repositories.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Core.Admin.Reports;
    using Core.Notification;
    using Domain.Reports;

    internal class FinancialGuaranteesRepository : IFinancialGuaranteesRepository
    {
        private readonly IwsContext context;

        public FinancialGuaranteesRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<FinancialGuaranteesData>> GetBlanketBonds(UKCompetentAuthority competentAuthority,
            string financialGuaranteeReferenceNumber, string exporterName, string importerName,
            string producerName)
        {
            return await context.Database.SqlQuery<FinancialGuaranteesData>(@"
                SELECT [ReferenceNumber]
                      ,[ApprovedDate]
                      ,[ActiveLoadsPermitted]
                      ,[CurrentActiveLoads]
                      ,[NotificationNumber]
                      ,[ExporterName]
                      ,[ImporterName]
                      ,[ProducerName]
                      ,[IsBlanketBond]
                  FROM [Reports].[BlanketBonds]
                 WHERE [CompetentAuthority] = @ca
                   AND (@financialGuaranteeReferenceNumber IS NULL OR [ReferenceNumber] LIKE '%' + @financialGuaranteeReferenceNumber + '%')
                   AND (@exporterName IS NULL OR [ExporterName] LIKE '%' + @exporterName + '%')
                   AND (@importerName IS NULL OR [ImporterName] LIKE '%' + @importerName + '%')
                   AND (@producerName IS NULL OR [ProducerName] LIKE '%' + @producerName + '%')",
                new SqlParameter("@ca", (int)competentAuthority),
                new SqlParameter("@financialGuaranteeReferenceNumber", (object)financialGuaranteeReferenceNumber ?? DBNull.Value),
                new SqlParameter("@exporterName", (object)exporterName ?? DBNull.Value),
                new SqlParameter("@importerName", (object)importerName ?? DBNull.Value),
                new SqlParameter("@producerName", (object)producerName ?? DBNull.Value)).ToArrayAsync();
        }
    }
}