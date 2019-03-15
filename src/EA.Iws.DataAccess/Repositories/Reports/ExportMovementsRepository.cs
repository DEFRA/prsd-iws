﻿namespace EA.Iws.DataAccess.Repositories.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Admin.Reports;
    using Core.Notification;
    using Domain.Reports;
    using EA.Iws.Core.Reports;
    using Newtonsoft.Json;

    internal class ExportMovementsRepository : IExportMovementsRepository
    {
        private readonly IwsContext context;

        public ExportMovementsRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<ExportMovementsData> Get(DateTime from, DateTime to, UKCompetentAuthority competentAuthority, OrganisationFilterOptions? organisationFilter, string organisationName)
        {
            string organisationJoin = organisationFilter == null ? string.Empty : string.Format("LEFT JOIN[Notification].[{0}] O ON M.NotificationId = O.NotificationId", organisationFilter.ToString());
            string organisationQuery = organisationFilter == null ? string.Empty : string.Format("AND O.Name LIKE '%{0}%'", organisationName);

            string query = string.Format(@"WITH 
                movementcreateddata AS(
                    SELECT M.NotificationId, IU.Id AS InternalUserId
                    FROM[Notification].[Movement] M
                        LEFT JOIN[Notification].[Notification] N ON M.NotificationId = N.Id
                        LEFT JOIN[Person].[InternalUser] IU ON M.CreatedBy = IU.UserId
                        {0}
                    WHERE N.CompetentAuthority = @ca
                        AND M.CreatedOnDate BETWEEN @from AND @to
                        {1}),

                receiptdata AS(
                    SELECT MR.*, IU.Id AS InternalUserId

                    FROM[Notification].[MovementReceipt] MR

                        INNER JOIN[Notification].[Movement] M ON M.Id = MR.MovementId

                        INNER JOIN[Notification].[Notification] N ON M.NotificationId = N.Id
                        LEFT JOIN[Person].[InternalUser] IU ON MR.CreatedBy = IU.UserId
                        {2}

                    WHERE N.CompetentAuthority = @ca

                        AND MR.CreatedOnDate BETWEEN @from AND @to
                        {3}),

                operationreceiptdata AS(
                    SELECT MOR.*, IU.Id AS InternalUserId

                    FROM[Notification].[MovementOperationReceipt] MOR

                        INNER JOIN[Notification].[Movement] M ON M.Id = MOR.MovementId

                        INNER JOIN[Notification].[Notification] N ON M.NotificationId = N.Id
                        LEFT JOIN[Person].[InternalUser] IU ON MOR.CreatedBy = IU.UserId
                        {4}

                    WHERE N.CompetentAuthority = @ca

                        AND MOR.CreatedOnDate BETWEEN @from AND @to
                        {5}),

                movementcreatedresult AS(
                    SELECT
                        COUNT(CASE WHEN InternalUserId IS NULL THEN 1 ELSE NULL END) AS MovementsCreatedExternally,
                        COUNT(InternalUserId) AS MovementsCreatedInternally
                    FROM movementcreateddata),

                receiptresult AS(
                    SELECT

                        COUNT(CASE WHEN InternalUserId IS NULL THEN 1 ELSE NULL END) AS MovementReceiptsCreatedExternally,
                        COUNT(CASE WHEN InternalUserId IS NOT NULL THEN 1 ELSE NULL END) AS MovementReceiptsCreatedInternally
                    FROM receiptdata),

                operationresult AS(
                    SELECT
                        COUNT(CASE WHEN InternalUserId IS NULL THEN 1 ELSE NULL END) AS MovementOperationReceiptsCreatedExternally,
                        COUNT(CASE WHEN InternalUserId IS NOT NULL THEN 1 ELSE NULL END) AS MovementOperationReceiptsCreatedInternally
                    FROM operationreceiptdata)

                SELECT * FROM movementcreatedresult, receiptresult, operationresult", organisationJoin, organisationQuery, organisationJoin, organisationQuery, organisationJoin, organisationQuery);

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ca", (int)competentAuthority),
                new SqlParameter("@from", from),
                new SqlParameter("@to", to)
            };

            if (organisationFilter != null)
            {
                parameters.Add(new SqlParameter("@org", organisationName));
            }

            var movementData = await context.Database.SqlQuery<ExportMovementsData>(query, parameters.ToArray()).SingleAsync();

            var userActions = await context.Database.SqlQuery<UserActionData>(
                @"SELECT
                    A.OriginalValue,
                    A.NewValue,
                    A.RecordId
                FROM[Auditing].[AuditLog] AS A
                INNER JOIN[Notification].[Movement] AS M ON M.Id = A.RecordId
                INNER JOIN[Notification].[Notification] AS N ON M.NotificationId = N.Id
                LEFT JOIN[Person].[InternalUser] AS IU ON A.UserId = IU.UserId
                WHERE TableName = '[Notification].[Movement]'
                    AND EventType != 0
                    AND IU.UserId IS NULL
                    AND N.CompetentAuthority = @ca
                    AND A.EventDate BETWEEN @from AND @to
                ORDER BY RecordId, EventDate",
                new SqlParameter("@ca", (int)competentAuthority),
                new SqlParameter("@from", from),
                new SqlParameter("@to", to)).ToListAsync();

            var movementsUploadedByExternalUser = 0;

            foreach (var notificationGroup in userActions.GroupBy(a => a.RecordId))
            {
                for (var i = 0; i < notificationGroup.Count(); i++)
                {
                    UserActionJsonModel o = JsonConvert.DeserializeObject<UserActionJsonModel>(notificationGroup.ElementAt(i).OriginalValue);
                    UserActionJsonModel n = JsonConvert.DeserializeObject<UserActionJsonModel>(notificationGroup.ElementAt(i).NewValue);

                    if (o.FileId == null && n.FileId != null)
                    {
                        movementsUploadedByExternalUser++;
                        i = notificationGroup.Count();
                    }
                }
            }

            movementData.FilesUploadedExternally = movementsUploadedByExternalUser;

            return movementData;
        }
    }
}