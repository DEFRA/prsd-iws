namespace EA.Iws.RequestHandlers.Notification
{
    using System;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GetExportNotificationsByUserHandler :
        IRequestHandler<GetExportNotificationsByUser, UserNotifications>
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;
        private readonly int pageSize = 20;

        public GetExportNotificationsByUserHandler(IwsContext context, IUserContext userContext)
        {
            this.context = context;
            this.userContext = userContext;
        }

        public async Task<UserNotifications> HandleAsync(GetExportNotificationsByUser message)
        {
            var notifications = await context.Database.SqlQuery<NotificationApplicationSummaryData>(@"
                SELECT 
                    N.Id,
                    N.NotificationNumber,
                    NA.Status,
                    COALESCE(NS.ChangeDate, N.CreatedDate) AS StatusDate,
                    E.Name AS Exporter,
                    I.Name AS Importer,
                    P.Name AS Producer
                FROM 
                    [Notification].[Notification] N
                    INNER JOIN [Notification].[NotificationAssessment] NA ON N.Id = NA.NotificationApplicationId
                    OUTER APPLY (
                        SELECT TOP 1 ChangeDate
                        FROM [Notification].[NotificationStatusChange] NS
                        WHERE NS.NotificationAssessmentId = NA.Id
                        ORDER BY ChangeDate DESC
                    ) NS
                    LEFT JOIN [Notification].[Exporter] E ON N.Id = E.NotificationId
                    LEFT JOIN [Notification].[Importer] I ON N.Id = I.NotificationId
                    LEFT JOIN [Notification].[ProducerCollection] PC ON N.Id = PC.NotificationId
                    LEFT JOIN [Notification].[Producer] P ON PC.Id = P.ProducerCollectionId AND P.IsSiteOfExport = 1
                WHERE 
                    N.UserId = @Id
                    AND (@Status IS NULL OR NA.Status = @Status)
                ORDER BY
                    N.CreatedDate DESC
                OFFSET (@Skip) ROWS FETCH NEXT (@Take) ROWS ONLY", 
                new SqlParameter("@Id", userContext.UserId),
                new SqlParameter("@Status", (object)message.NotificationStatus ?? DBNull.Value),
                new SqlParameter("@Skip", (message.PageNumber - 1) * pageSize),
                new SqlParameter("@Take", pageSize)).ToListAsync();

            var numberOfNotifications = await context.Database.SqlQuery<int>(
                @"SELECT COUNT(N.[Id])
                  FROM [Notification].[Notification] N
                  INNER JOIN [Notification].[NotificationAssessment] NA ON N.Id = NA.NotificationApplicationId
                  WHERE N.UserId = @Id AND (@Status IS NULL OR NA.Status = @Status)",
                new SqlParameter("@Id", userContext.UserId),
                new SqlParameter("@Status", (object)message.NotificationStatus ?? DBNull.Value)).SingleAsync();

            return new UserNotifications(notifications, numberOfNotifications, message.PageNumber, pageSize);
        }
    }
}