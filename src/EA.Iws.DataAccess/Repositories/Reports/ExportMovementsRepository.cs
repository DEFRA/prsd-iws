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
            var movementData = await context.Database.SqlQuery<ExportMovementsData>(
                @"WITH MovementAudit AS
                   (SELECT 
                        IU.Id AS UserId,
						MR.Date AS MRReceiptDate,
						MOR.Date AS MORReceiptDate,
						A.TableName,
						N.Id AS bob
                    FROM Auditing.AuditLog AS A 
                        LEFT JOIN [Notification].[Movement] AS M ON A.RecordId = M.Id
						LEFT JOIN [Notification].[MovementReceipt] AS MR ON A.RecordId = MR.Id
						LEFT JOIN [Notification].[MovementOperationReceipt] AS MOR ON A.RecordId = MOR.Id
                        LEFT JOIN [Notification].[Notification] N ON M.NotificationId = N.Id
                        LEFT JOIN [Identity].[AspNetUsers] U ON M.CreatedBy = U.Id OR MR.CreatedBy = U.Id OR MOR.CreatedBy = U.Id
							LEFT JOIN [Person].[InternalUser] IU ON U.Id = IU.UserId
                    WHERE A.EventType = 0 
                        AND (A.TableName = '[Notification].[Movement]' OR A.TableName = '[Notification].[MovementReceipt]' OR A.TableName = '[Notification].[MovementOperationReceipt]')
                        AND (N.CompetentAuthority = @ca OR (A.TableName != '[Notification].[Movement]' AND N.CompetentAuthority IS NULL))
                        AND A.EventDate BETWEEN @from AND @to)

                    SELECT 
                        COUNT(CASE WHEN UserId IS NULL AND TableName = '[Notification].[Movement]' THEN 1 ELSE NULL END) AS MovementsCreatedExternally,
                        COUNT(CASE WHEN UserId IS NOT NULL AND TableName = '[Notification].[Movement]' THEN 1 ELSE NULL END) AS MovementsCreatedInternally,
						COUNT(CASE WHEN UserId IS NULL AND MRReceiptDate IS NOT NULL THEN 1 ELSE NULL END) AS MovementReceiptsCreatedExternally,
                        COUNT(CASE WHEN UserId IS NOT NULL AND MRReceiptDate IS NOT NULL THEN 1 ELSE NULL END) AS MovementReceiptsCreatedInternally,
						COUNT(CASE WHEN UserId IS NULL AND MORReceiptDate IS NOT NULL THEN 1 ELSE NULL END) AS MovementOperationReceiptsCreatedExternally,
                        COUNT(CASE WHEN UserId IS NOT NULL AND MORReceiptDate IS NOT NULL THEN 1 ELSE NULL END) AS MovementOperationReceiptsCreatedInternally
                    FROM MovementAudit",
                    new SqlParameter("@ca", (int)competentAuthority),
                    new SqlParameter("@from", from),
                    new SqlParameter("@to", to)).SingleAsync();

            var result = new ExportMovementsData();
            result.MovementsCreatedExternally = movementData.MovementsCreatedExternally;
            result.MovementsCreatedInternally = movementData.MovementsCreatedInternally;
            result.MovementReceiptsCreatedExternally = movementData.MovementReceiptsCreatedExternally;
            result.MovementReceiptsCreatedInternally = movementData.MovementReceiptsCreatedInternally;
            result.MovementOperationReceiptsCreatedExternally = movementData.MovementOperationReceiptsCreatedExternally;
            result.MovementOperationReceiptsCreatedInternally = movementData.MovementOperationReceiptsCreatedInternally;

            var userActions = await context.Database.SqlQuery<UserActionData>(
                @"SELECT     
	                A.NewValue,
	                A.RecordId
                FROM [Auditing].[AuditLog] AS A
                INNER JOIN [Notification].[Movement] AS M ON M.Id = A.RecordId
                INNER JOIN [Notification].[Notification] N ON M.NotificationId = N.Id
                INNER JOIN [Identity].[AspNetUsers] U ON A.UserId = U.Id
                LEFT JOIN [Person].[InternalUser] IU ON U.Id = IU.UserId
                WHERE TableName = '[Notification].[Movement]' 
                    AND EventType != 0 
                    AND IU.UserId IS NULL
                    AND N.CompetentAuthority = @ca
                    AND A.EventDate BETWEEN @from AND @to
                ORDER BY RecordId, EventDate",
                new SqlParameter("@ca", (int)competentAuthority),
                new SqlParameter("@from", from),
                new SqlParameter("@to", to)).ToListAsync();

            return result;
        }
    }
}