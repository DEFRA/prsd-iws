namespace EA.Iws.DataAccess.Repositories.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Core.Notification;
    using Domain.Reports;

    internal class ExportStatsRepository : IExportStatsRepository
    {
        private readonly IwsContext context;

        public ExportStatsRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<ExportStats>> GetExportStats(DateTime from, DateTime to, UKCompetentAuthority competentAuthority)
        {
            return await context.Database.SqlQuery<ExportStats>(
                @"SELECT
                      SUM(QuantityReceived) AS QuantityReceived,
                      CASE 
                          WHEN YCode IS NULL THEN 'No applicable Y code'
                          ELSE YCode
                      END AS WasteCategory,
                      WasteStreams,
                      CountryOfImport,
                      TransitStates,
                      [BaselOecd],
                      [EWC],
                      [Hcode],
                      [HcodeDescription],
                      [UN],
                      [RCode],
                      [DCode]
                  FROM
                      [Reports].[ExportStats]
                  WHERE [ReceivedDate] BETWEEN @from AND @to
                      AND [CompetentAuthority] = @ca
                  GROUP BY
                      [YCode],
                      [WasteStreams],
                      [CountryOfImport],
                      TransitStates,
                      [BaselOecd],
                      [EWC],
                      [Hcode],
                      [HcodeDescription],
                      [UN],
                      [RCode],
                      [DCode]",
                new SqlParameter("@from", from),
                new SqlParameter("@to", to),
                new SqlParameter("@ca", (int)competentAuthority)).ToArrayAsync();
        }
    }
}