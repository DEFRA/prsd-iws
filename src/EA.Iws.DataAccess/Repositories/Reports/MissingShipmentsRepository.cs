namespace EA.Iws.DataAccess.Repositories.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Core.Notification;
    using Domain.Reports;

    internal class MissingShipmentsRepository : IMissingShipmentsRepository
    {
        private readonly IwsContext context;

        public MissingShipmentsRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<MissingShipment>> Get(DateTime from, DateTime to, UKCompetentAuthority competentAuthority)
        {
            return await context.Database.SqlQuery<MissingShipment>(
                @"SELECT 
                    [NotificationNumber],
                    [Exporter],
                    [Importer],
                    [Facility],
                    [ShipmentNumber],
                    [ActualDateOfShipment],
                    [ConsentFrom],
                    [ConsentTo],
                    [PrenotificationDate],
                    [ReceivedDate],
                    [CompletedDate],
                    [QuantityReceived],
                    [QuantityReceivedUnitId] AS [Units],
                    [ChemicalComposition],
                    [LocalArea]
                FROM [Reports].[NotificationShipmentDataMissingShipments]
                WHERE [CompetentAuthorityId] = @ca
                AND COALESCE([PrenotificationDate], [ActualDateOfShipment]) BETWEEN @from AND @to",
                new SqlParameter("@from", from),
                new SqlParameter("@to", to),
                new SqlParameter("@ca", (int)competentAuthority)).ToArrayAsync();
        }
    }
}
