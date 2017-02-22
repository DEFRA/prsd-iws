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
            ChemicalComposition? chemicalComposition, UKCompetentAuthority competentAuthority, FoiReportDates dateType)
        {
            return await context.Database.SqlQuery<FreedomOfInformationData>(
                @"SELECT DISTINCT
                    [NotificationNumber],
                    [ImportOrExport],
                    CASE WHEN [IsInterim] = 1 THEN 'Interim' WHEN [IsInterim] = 0 THEN 'Non-interim' ELSE NULL END AS [Interim],
                    [NotifierName],
                    [NotifierAddress],
                    [NotifierPostalCode],
                    [ProducerName],
                    [ProducerAddress],
                    [ProducerPostalCode],
                    [PointOfExport],
                    [PointOfEntry],
                    [ImportCountryName],
                    [NameOfWaste],
                    [EWC],
                    [YCode],
                    [HCode],
                    [OperationCodes],
                    [ImporterName],
                    [ImporterAddress],
                    [ImporterPostalCode],
                    [FacilityName],
                    [FacilityAddress],
                    [FacilityPostalCode],
                    COALESCE(
                        (SELECT	SUM(
                            CASE WHEN [MovementQuantityReceviedUnitId] IN (1, 2) -- Tonnes / Cubic Metres
                                THEN COALESCE([MovementQuantityReceived], 0)
                            ELSE 
                                COALESCE([MovementQuantityReceived] / 1000, 0) -- Convert to Tonnes / Cubic Metres
                            END
                            )
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
                    [Reports].[FreedomOfInformationCache]
                WHERE 
                    [CompetentAuthorityId] = @competentAuthority
                    AND (@chemicalComposition IS NULL OR [ChemicalCompositionTypeId] = @chemicalComposition)
                    AND (@dateType = 'NotificationReceivedDate' AND  [ReceivedDate] BETWEEN @from AND @to
                         OR @dateType = 'ConsentFrom' AND  [ConsentFrom] BETWEEN @from AND @to
                         OR @dateType = 'ReceivedDate' AND [MovementReceivedDate] BETWEEN @from AND @to
                         OR @dateType = 'CompletedDate' AND [MovementCompletedDate] BETWEEN @from AND @to)
                GROUP BY
                    [NotificationNumber],
                    [ImportOrExport],
                    [IsInterim],
                    [NotifierName],
                    [NotifierAddress],
                    [NotifierPostalCode],
                    [ProducerName],
                    [ProducerAddress],
                    [ProducerPostalCode],
                    [PointOfExport],
                    [PointOfEntry],
                    [ImportCountryName],
                    [NameOfWaste],
                    [EWC],
                    [YCode],
                    [HCode],
                    [OperationCodes],
                    [ImporterName],
                    [ImporterAddress],
                    [ImporterPostalCode],
                    [FacilityName],
                    [FacilityAddress],
                    [FacilityPostalCode],
                    [IntendedQuantityUnitId],
                    [IntendedQuantityUnit],
                    [IntendedQuantity],
                    [IntendedQuantityUnit],
                    [ConsentFrom],
                    [ConsentTo],
                    [LocalArea]",
                new SqlParameter("@from", from),
                new SqlParameter("@to", to),
                new SqlParameter("@chemicalComposition", (chemicalComposition.HasValue ? (object)(int)chemicalComposition.Value : DBNull.Value)),
                new SqlParameter("@competentAuthority", (int)competentAuthority),
                new SqlParameter("@dateType", dateType.ToString())).ToArrayAsync();
        }
    }
}