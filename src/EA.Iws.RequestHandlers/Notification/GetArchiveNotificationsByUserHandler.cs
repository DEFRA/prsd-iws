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
        private readonly int pageSize = 25;

        public GetArchiveNotificationsByUserHandler(IwsContext context, IUserContext userContext)
        {
            this.context = context;
            this.userContext = userContext;
        }

        //List any notification in table that has a final decision/status (File closed, Withdrawn, Objected and Consent Withdrawn)
        //and is greater than 3 years from the current date.
        //User should only see notifications from companies that relate to their appropriate authority
        //Order the list showing the oldest at the top to the newest, based on generated date.
        public async Task<UserArchiveNotifications> HandleAsync(GetArchiveNotificationsByUser message)
        {
            var notifications = await context.Database.SqlQuery<NotificationArchiveSummaryData>(
                "[Notification].[uspGetArchiveNotificationsByUser] @UserId, @Skip, @Take",
                new SqlParameter("@UserId", userContext.UserId),
                new SqlParameter("@Skip", (message.PageNumber - 1) * pageSize),
                new SqlParameter("@Take", pageSize)).ToListAsync();

            var numberOfNotifications = await context.Database.SqlQuery<int>("[Notification].[uspGetArchiveNotificationsCountByUser] @UserId",
                new SqlParameter("@UserId", userContext.UserId)).SingleAsync();

            return new UserArchiveNotifications(notifications, numberOfNotifications, message.PageNumber, pageSize);
        }
    }
}