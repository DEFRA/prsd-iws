namespace EA.Iws.DataAccess.Repositories.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Core.Admin.Reports;
    using Core.Notification;
    using Core.WasteType;
    using Domain.Reports;

    internal class RdfSrfWoodRepository : IRdfSrfWoodRepository
    {
        private readonly IwsContext context;

        public RdfSrfWoodRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<RdfSrfWoodData>> Get(DateTime from, DateTime to,
            ChemicalComposition chemicalComposition, UKCompetentAuthority competentAuthority)
        {
            return await context.Database.SqlQuery<RdfSrfWoodData>(
                @"SELECT 
                    [NotifierName],
                    [NotifierAddress],
                    [ProducerName],
                    [ProducerAddress],
                    [PointOfExport],
                    [NameOfWaste],
                    [EWC],
                    [YCode],
                    [FacilityName],
                    [FacilityAddress],
                    SUM([QuantityReceived]) AS [QuantityReceived],
                    [QuantityReceivedUnit]
                FROM 
                    [Reports].[RdfSrfWood]
                WHERE 
                    [CompetentAuthorityId] = @competentAuthority
                    AND [ChemicalCompositionTypeId] = @chemicalComposition
                    AND [ReceivedDate] BETWEEN @from AND @to
                GROUP BY
                    [NotifierName],
                    [NotifierAddress],
                    [ProducerName],
                    [ProducerAddress],
                    [PointOfExport],
                    [NameOfWaste],
                    [EWC],
                    [YCode],
                    [FacilityName],
                    [FacilityAddress],
                    [QuantityReceivedUnit]",
                new SqlParameter("@from", from),
                new SqlParameter("@to", to),
                new SqlParameter("@chemicalComposition", (int)chemicalComposition),
                new SqlParameter("@competentAuthority", (int)competentAuthority)).ToArrayAsync();
        }
    }
}