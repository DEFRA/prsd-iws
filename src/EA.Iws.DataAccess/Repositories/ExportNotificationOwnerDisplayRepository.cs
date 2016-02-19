namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Notification;
    using Core.NotificationAssessment;
    using Domain.NotificationApplication;

    internal class ExportNotificationOwnerDisplayRepository : IExportNotificationOwnerDisplayRepository
    {
        private readonly IwsContext context;

        public ExportNotificationOwnerDisplayRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<ExportNotificationOwnerDisplay>> GetInternalUnsubmittedByCompetentAuthority(UKCompetentAuthority competentAuthority)
        {
            return await context.Database.SqlQuery<ExportNotificationOwnerDisplay>(@"
                SELECT 
                    N.Id AS NotificationId,
                    N.NotificationNumber AS Number,
                    E.Name AS ExporterName,
                    I.Name AS ImporterName,
                    P.Name AS ProducerName,
                    ANU.FirstName + ' ' + ANU.Surname AS OwnerName
                FROM 
                    [Notification].[Notification] N
                    INNER JOIN [Notification].[NotificationAssessment] NA ON N.Id = NA.NotificationApplicationId AND NA.Status = 1
                    LEFT JOIN [Notification].[Exporter] E ON N.Id = E.NotificationId
                    LEFT JOIN [Notification].[Importer] I ON N.Id = I.NotificationId
                    LEFT JOIN [Notification].[ProducerCollection] PC ON N.Id = PC.NotificationId
                    LEFT JOIN [Notification].[Producer] P ON PC.Id = P.ProducerCollectionId AND P.IsSiteOfExport = 1
                    INNER JOIN [Person].[InternalUser] IU ON IU.UserId = N.UserId
                    LEFT JOIN [Identity].[AspNetUsers] ANU ON ANU.Id = IU.UserId
                WHERE 
                    N.CompetentAuthority = @Ca
                ORDER BY
                    N.NotificationNumber ASC",
                new SqlParameter("@Ca", competentAuthority)).ToListAsync();
        }
    }
}