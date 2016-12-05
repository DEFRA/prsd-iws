namespace EA.Iws.DataAccess.Repositories.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Core.Admin.Reports;
    using Core.Notification;
    using Core.Reports;
    using Core.WasteType;
    using Domain.Reports;

    internal class FreedomOfInformationRepository : IFreedomOfInformationRepository
    {
        private readonly IwsContext context;

        public FreedomOfInformationRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<FreedomOfInformationData>> Get(DateTime from, DateTime to,
            ChemicalComposition chemicalComposition, UKCompetentAuthority competentAuthority)
        {
            return await context.Database.SqlQuery<FreedomOfInformationData>(
                @"SELECT 
                    [NotificationNumber],
                    [NotifierName],
                    [NotifierAddress],
                    [ProducerName],
                    [ProducerAddress],
                    [PointOfExport],
                    [PointOfEntry],
                    [ImportCountryName],
                    [NameOfWaste],
                    [EWC],
                    [YCode],
                    [OperationCodes],
                    [ImporterName],
                    [ImporterAddress],
                    [FacilityName],
                    [FacilityAddress],
                    COALESCE(                    
                        (SELECT	SUM(
                            CASE WHEN [QuantityReceivedUnitId] IN (1, 2) -- Tonnes / Cubic Metres
                                THEN COALESCE([QuantityReceived], 0)
                            ELSE 
                                COALESCE([QuantityReceived] / 1000, 0) -- Convert to Tonnes / Cubic Metres
                            END
                            ) 
                            FROM [Reports].[Movements]
                            WHERE Id = NotificationId
                        ), 0) AS [QuantityReceived],
                    CASE WHEN [IntendedQuantityUnitId] IN (1, 2) -- Due to conversion units will only be Tonnes / Cubic Metres
                        THEN [IntendedQuantityUnit] 
                    WHEN [IntendedQuantityUnitId] = 3 THEN 'Tonnes'
                    WHEN [IntendedQuantityUnitId] = 4 THEN 'Cubic Metres'
                    END AS [QuantityReceivedUnit],
                    [IntendedQuantity],
                    [IntendedQuantityUnit],
                    [ConsentFrom],
                    [ConsentTo],
                    [LocalArea]
                FROM 
                    [Reports].[FreedomOfInformation]
                WHERE 
                    [CompetentAuthorityId] = @competentAuthority
                    AND [ChemicalCompositionTypeId] = @chemicalComposition
                    AND [ReceivedDate] BETWEEN @from AND @to",
                new SqlParameter("@from", from),
                new SqlParameter("@to", to),
                new SqlParameter("@chemicalComposition", (int)chemicalComposition),
                new SqlParameter("@competentAuthority", (int)competentAuthority)).ToArrayAsync();
        }
    }
}