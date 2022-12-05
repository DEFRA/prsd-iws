namespace EA.Iws.RequestHandlers.Notification
{
    using EA.Iws.DataAccess;
    using EA.Iws.Requests.Notification;
    using EA.Prsd.Core.Domain;
    using EA.Prsd.Core.Mediator;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    internal class GetArchiveNotificationsByUserHandler : IRequestHandler<GetArchiveNotificationsByUser, UserArchiveNotifications>
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;
        private readonly int pageSize = 20;

        public GetArchiveNotificationsByUserHandler(IwsContext context, IUserContext userContext)
        {
            this.context = context;
            this.userContext = userContext;
        }

        //List any notification in table that has a final decision/status (File closed, Withdrawn, Objected and Consent Withdrawn)
        //and is greater than 3 years from the current date.
        //User should only see notifications from companies that relate to their appropriate authority
        //Order the list showing the oldest at the top to the newest, based on generated date.
        //OrganisationName
        public async Task<UserArchiveNotifications> HandleAsync(GetArchiveNotificationsByUser message)
        {
            //Modify to Stored Procedure
            //Add to C:\Workspace\prsd-iws\src\EA.Iws.Database\scripts\Everytime\03-StoredProcedures\

            //Needs to be export and import notifications

            //[Notification],[notification] union with [ImportNotification].[Notification]
            //TODO JR use E.Name AS Exporter, for company name
            //TODO JR filter by user competent authority
            //TODO JR filter by dateCreated > 3 years

            var notifications = await context.Database.SqlQuery<NotificationArchiveSummaryData>("[Notification].[uspGetArchiveNotificationsByUser] @UserId, @Skip, @Take",
                new SqlParameter("@UserId", userContext.UserId),
                new SqlParameter("@Skip", (message.PageNumber - 1) * pageSize),
                new SqlParameter("@Take", pageSize)).ToListAsync();

            //TODO JR Fix this to show the number of notifications overall.
            var numberOfNotifications = await context.Database.SqlQuery<int>(
                @"SELECT COUNT(N.[Id])
                  FROM [Notification].[Notification] N
                  INNER JOIN [Notification].[NotificationAssessment] NA ON N.Id = NA.NotificationApplicationId
                  LEFT JOIN [Notification].[SharedUser] SU ON SU.NotificationId = N.Id AND SU.UserId =  @Id
                  WHERE (N.UserId = @Id OR SU.UserId = @Id) ",
                new SqlParameter("@Id", userContext.UserId)).SingleAsync();

            return new UserArchiveNotifications(notifications, numberOfNotifications, message.PageNumber, pageSize);
        }
    }
}