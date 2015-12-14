namespace EA.Iws.DataAccess.Repositories.Reports
{
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Domain;
    using Domain.Reports;

    internal class MissingShipmentsRepository : IMissingShipmentsRepository
    {
        private readonly IwsContext context;

        public MissingShipmentsRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<MissingShipment>> Get(int year, UKCompetentAuthority competentAuthority)
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
                WHERE [CompetentAuthorityId] = @ca",
                new SqlParameter("@year", year),
                new SqlParameter("@ca", competentAuthority.Value)).ToArrayAsync();
        }
    }
}
