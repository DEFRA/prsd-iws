namespace EA.Iws.DataAccess.Repositories.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Core.Admin.Reports;
    using Core.Notification;
    using Core.Reports;
    using Core.Reports.FOI;
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
             UKCompetentAuthority competentAuthority, FOIReportDates dateType, FOIReportTextFields? searchField,
            TextFieldOperator? searchType,
            string comparisonText)
        {
            var textFilter = TextFilterHelper.GetTextFilter(searchField, searchType, comparisonText);
            textFilter = !string.IsNullOrEmpty(textFilter) ? string.Format("AND {0}", textFilter) : string.Empty;

            var query = @"SELECT DISTINCT
                    [NotificationNumber],
                    [ImportOrExport],
                    CASE WHEN [IsInterim] = 1 THEN 'Interim' WHEN [IsInterim] = 0 THEN 'Non-interim' ELSE NULL END AS [Interim],
                    [NotifierName],
                    [NotifierAddress],
                    [NotifierPostalCode],
                    [NotifierType],
                    [NotifierContactName],
                    [NotifierContactEmail],
                    [ProducerName],
                    [ProducerAddress],
                    [ProducerPostalCode],
                    [ProducerType],
                    [ProducerContactEmail],
                    [PointOfExport],
                    [PointOfEntry],
                    [ExportCountryName],
                    [ImportCountryName],
                    [TransitStates],
                    [BaselOecdCode],
                    [NameOfWaste],
                    [EWC],
                    [YCode],
                    [HCode],
                    [OperationCodes],
                    [ImporterName],
                    [ImporterAddress],
                    [ImporterPostalCode],
                    [ImporterType],
                    [ImporterContactName],
                    [ImporterContactEmail],
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
                    [NotificationStatus],
                    [DecisionRequiredByDate],
                    [IsFinancialGuaranteeApproved],
                    [FileClosedDate],
                    [LocalArea],
                    [TechnologyEmployed]
                FROM 
                    [Reports].[FreedomOfInformationCache]
                WHERE 
                    [CompetentAuthorityId] = @competentAuthority
                    AND (@dateType = 'NotificationReceivedDate' AND  [ReceivedDate] BETWEEN @from AND @to
                         OR @dateType = 'ConsentFrom' AND  [ConsentFrom] BETWEEN @from AND @to
                         OR @dateType = 'ConsentTo' AND  [ConsentTo] BETWEEN @from AND @to
                         OR @dateType = 'ReceivedDate' AND [MovementReceivedDate] BETWEEN @from AND @to
                         OR @dateType = 'CompletedDate' AND [MovementCompletedDate] BETWEEN @from AND @to
                         OR @dateType = 'ActualDate' AND [ActualDate] BETWEEN @from AND @to
                         OR @dateType = 'DecisionDate' AND [DecisionRequiredByDate] BETWEEN @from AND @to
                         OR @dateType = 'AcknowledgedDate' AND [AcknowledgedDate] BETWEEN @from AND @to
                         OR @dateType = 'ObjectionDate' AND [ObjectionDate] BETWEEN @from AND @to
                         OR @dateType = 'FileClosedDate' AND [FileClosedDate] BETWEEN @from AND @to
                         OR @dateType = 'WithdrawnDate' AND [WithdrawnDate] BETWEEN @from AND @to)    
                     {0}
                GROUP BY
                    [NotificationNumber],
                    [ImportOrExport],
                    [IsInterim],
                    [NotifierName],
                    [NotifierAddress],
                    [NotifierPostalCode],
                    [NotifierType],
                    [NotifierContactName],
                    [NotifierContactEmail],
                    [ProducerName],
                    [ProducerAddress],
                    [ProducerPostalCode],
                    [ProducerType],
                    [ProducerContactEmail],
                    [PointOfExport],
                    [PointOfEntry],
                    [ExportCountryName],
                    [ImportCountryName],
                    [TransitStates],
                    [BaselOecdCode],
                    [NameOfWaste],
                    [EWC],
                    [YCode],
                    [HCode],
                    [OperationCodes],
                    [ImporterName],
                    [ImporterAddress],
                    [ImporterPostalCode],
                    [ImporterType],
                    [ImporterContactName],
                    [ImporterContactEmail],
                    [FacilityName],
                    [FacilityAddress],
                    [FacilityPostalCode],
                    [IntendedQuantityUnitId],
                    [IntendedQuantityUnit],
                    [IntendedQuantity],
                    [IntendedQuantityUnit],
                    [ConsentFrom],
                    [ConsentTo],
                    [NotificationStatus],
                    [DecisionRequiredByDate],
                    [IsFinancialGuaranteeApproved],
                    [FileClosedDate],
                    [LocalArea],
                    [TechnologyEmployed]";

              return await context.Database.SqlQuery<FreedomOfInformationData>(string.Format(query, textFilter),
                new SqlParameter("@from", from),
                new SqlParameter("@to", to),
                new SqlParameter("@competentAuthority", (int)competentAuthority),
                new SqlParameter("@dateType", dateType.ToString())).ToArrayAsync();
        }
    }
}