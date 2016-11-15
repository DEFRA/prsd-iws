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
                    COALESCE([QuantityReceived], 0) AS [QuantityReceived],
                    [QuantityReceivedUnit],
                    [IntendedQuantity],
                    [IntendedQuantityUnit],
                    [ConsentFrom],
                    [ConsentTo]
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