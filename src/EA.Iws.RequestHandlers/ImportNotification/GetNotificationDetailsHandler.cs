namespace EA.Iws.RequestHandlers.ImportNotification
{
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Core.ImportNotification;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;

    internal class GetNotificationDetailsHandler : IRequestHandler<GetNotificationDetails, NotificationDetails>
    {
        private readonly ImportNotificationContext context;

        public GetNotificationDetailsHandler(ImportNotificationContext context)
        {
            this.context = context;
        }

        public async Task<NotificationDetails> HandleAsync(GetNotificationDetails message)
        {
            return await context.Database.SqlQuery<NotificationDetails>(
                @"SELECT
                    N.[Id] AS [ImportNotificationId],
                    N.[NotificationNumber],
                    N.[NotificationType],
                    N.[CompetentAuthority],
                    NA.[Status],
                    LA.[Name] AS [Area],
                    I.[IsInterim],
                    FC.[AllFacilitiesPreconsented]
                FROM
                    [ImportNotification].[Notification] N
                    INNER JOIN [ImportNotification].[NotificationAssessment] NA ON NA.[NotificationApplicationId] = N.[Id]
                    LEFT JOIN [ImportNotification].[Consultation] C 
                        INNER JOIN [Lookup].[LocalArea] LA ON LA.[Id] = C.[LocalAreaId]
                    ON C.[NotificationId] = N.[Id]
                    INNER JOIN [ImportNotification].[InterimStatus] I ON I.[ImportNotificationId] = N.[Id]
                    LEFT JOIN [ImportNotification].[FacilityCollection] FC ON FC.[ImportNotificationId] = N.[Id]
                WHERE
                    N.[Id] = @notificationId",
                new SqlParameter("@notificationId", message.ImportNotificationId)).SingleAsync();
        }
    }
}