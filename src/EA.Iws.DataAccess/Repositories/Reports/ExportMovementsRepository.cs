namespace EA.Iws.DataAccess.Repositories.Reports
{
    using System;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Core.Admin.Reports;
    using Core.Notification;
    using Domain.Reports;

    internal class ExportMovementsRepository : IExportMovementsRepository
    {
        private readonly IwsContext context;

        public ExportMovementsRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<ExportMovementsData> Get(DateTime from, DateTime to, UKCompetentAuthority competentAuthority)
        {
            return await context.Database.SqlQuery<ExportMovementsData>(
                @"SELECT
                      COUNT(CASE WHEN MovementInternalUserId IS NULL AND FileId IS NOT NULL THEN 1 ELSE NULL END) AS FilesUploadedExternally,
                      COUNT(CASE WHEN MovementInternalUserId IS NOT NULL AND FileId IS NOT NULL THEN 1 ELSE NULL END) AS FilesUploadedInternally,
                      COUNT(CASE WHEN MovementInternalUserId IS NULL THEN 1 ELSE NULL END) AS MovementsCreatedExternally,
                      COUNT(MovementInternalUserId) AS MovementsCreatedInternally,
                      COUNT(CASE WHEN MovementReceiptInternalUserId IS NULL AND ReceiptDate IS NOT NULL THEN 1 ELSE NULL END) AS MovementReceiptsCreatedExternally,
                      COUNT(CASE WHEN MovementReceiptInternalUserId IS NOT NULL AND ReceiptDate IS NOT NULL THEN 1 ELSE NULL END) AS MovementReceiptsCreatedInternally,
                      COUNT(CASE WHEN MovementOperationReceiptInternalUserId IS NULL AND OperationDate IS NOT NULL THEN 1 ELSE NULL END) AS MovementOperationReceiptsCreatedExternally,
                      COUNT(CASE WHEN MovementOperationReceiptInternalUserId IS NOT NULL AND OperationDate IS NOT NULL THEN 1 ELSE NULL END) AS MovementOperationReceiptsCreatedInternally
                  FROM
                      [Reports].[ExportMovements]
                  WHERE 
                      [CompetentAuthority] = @ca
                      AND ShipmentDate BETWEEN @from AND @to",
                new SqlParameter("@ca", (int)competentAuthority),
                new SqlParameter("@from", from),
                new SqlParameter("@to", to)).SingleAsync();
        }
    }
}