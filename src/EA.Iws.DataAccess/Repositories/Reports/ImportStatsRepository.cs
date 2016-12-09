namespace EA.Iws.DataAccess.Repositories.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Core.Notification;
    using Domain.Reports;

    internal class ImportStatsRepository : IImportStatsRepository
    {
        private readonly IwsContext context;

        public ImportStatsRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<ImportStats>> GetImportStats(DateTime @from, DateTime to,
            UKCompetentAuthority competentAuthority)
        {
            return await context.Database.SqlQuery<ImportStats>(
                @"SELECT
                    [QuantityReceived],
                    [WasteCategory],
                    [WasteStreams],
                    [CountryOfExport],
                    [TransitStates],
                    [BaselOecd],
                    [EWC],
                    [Hcode],
                    [HcodeDescription],
                    [UN],
                    [RCode],
                    [DCode]
                FROM [Reports].[ImportStats]
                WHERE [ReceivedDate] BETWEEN @from AND @to
                AND [CompetentAuthority] = @ca",
                new SqlParameter("@from", from),
                new SqlParameter("@to", to),
                new SqlParameter("@ca", (int)competentAuthority)).ToArrayAsync();
        }
    }
}