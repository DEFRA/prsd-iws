namespace EA.Iws.RequestHandlers.Notification
{
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GetNotificationUpdateHistoryHandler :
        IRequestHandler<GetNotificationUpdateHistory, NotificationUpdateHistory>
    {
        private readonly IwsContext context;

        public GetNotificationUpdateHistoryHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<NotificationUpdateHistory> HandleAsync(GetNotificationUpdateHistory message)
        {
            var notificationUpdateHistory = await context.Database.SqlQuery<NotificationUpdateHistorySummaryData>(@"
                SELECT
	                NEWID() AS Id,
	                CASE WHEN IU.Id IS NULL THEN U.FirstName + ' ' + U.Surname ELSE 'Internal User' END AS Name,
	                FORMAT(A.DateAdded, 'yyyy-MM-dd') AS Date,
	                FORMAT(A.DateAdded, 'hh:mm:ss') AS Time,
	                S.ScreenName AS InformationChange,
	                AT.AuditType AS TypeOfChange
                FROM 
                    [Notification].[Notification] N
                    INNER JOIN [Notification].[Audit] A ON N.Id = A.NotificationId
                    INNER JOIN [Identity].[AspNetUsers] U ON A.UserId = U.Id
                    LEFT JOIN [Person].[InternalUser] IU ON U.Id = IU.UserId
                    INNER JOIN [Lookup].[Screen] S ON A.Screen = S.Id
                    INNER JOIN [Lookup].[AuditType] AT ON A.Type = AT.Id
                WHERE 
                    N.Id = @NotificationId
                ORDER BY 
                    A.DateAdded DESC",
            new SqlParameter("@NotificationId", message.NotificationId)).ToListAsync();

            return new NotificationUpdateHistory(notificationUpdateHistory);
        }
    }
}
