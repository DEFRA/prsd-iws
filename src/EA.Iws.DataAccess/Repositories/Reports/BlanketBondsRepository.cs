namespace EA.Iws.DataAccess.Repositories.Reports
{
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Core.Admin.Reports;
    using Core.Notification;
    using Domain.Reports;

    internal class BlanketBondsRepository : IBlanketBondsRepository
    {
        private readonly IwsContext context;

        public BlanketBondsRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<BlanketBondsData>> GetBlanketBonds(UKCompetentAuthority competentAuthority,
            string financialGuaranteeReferenceNumber, string exporterName, string importerName,
            string producerName)
        {
            return await context.Database.SqlQuery<BlanketBondsData>(@"
                SELECT [ReferenceNumber]
                      ,[ApprovedDate]
                      ,[ActiveLoadsPermitted]
                      ,[CurrentActiveLoads]
                      ,[NotificationNumber]
                      ,[ExporterName]
                      ,[ImporterName]
                      ,[ProducerName]
                  FROM [Reports].[BlanketBonds]
                 WHERE [CompetentAuthority] = @ca
                   AND (@financialGuaranteeReferenceNumber IS NULL OR [ReferenceNumber] = @financialGuaranteeReferenceNumber)
                   AND (@exporterName IS NULL OR [ExporterName] = @exporterName)
                   AND (@importerName IS NULL OR [ImporterName] = @importerName)
                   AND (@producerName IS NULL OR [ProducerName] = @producerName)",
                new SqlParameter("@ca", (int)competentAuthority),
                new SqlParameter("@exporterName", exporterName),
                new SqlParameter("@importerName", importerName),
                new SqlParameter("@producerName", producerName)).ToArrayAsync();
        }
    }
}