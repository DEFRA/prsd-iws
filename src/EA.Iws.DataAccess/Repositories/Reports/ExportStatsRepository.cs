namespace EA.Iws.DataAccess.Repositories.Reports
{
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Domain.Reports;

    internal class ExportStatsRepository : IExportStatsRepository
    {
        private readonly IwsContext context;

        public ExportStatsRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<ExportStats>> GetExportStats(int year)
        {
            return await context.Database.SqlQuery<ExportStats>(
                @"SELECT 
                    [QuantityReceived],
                    [WasteCategory],
                    [WasteStreams],
                    [CountryOfImport],
                    [TransitStates],
                    [BaselOecd],
                    [EWC],
                    [Hcode],
                    [HcodeDescription],
                    [UN],
                    [RCode],
                    [DCode]
                FROM [Reports].[ExportStats]
                WHERE [Year] = @year",
                new SqlParameter("@year", year)).ToArrayAsync();
        }
    }
}