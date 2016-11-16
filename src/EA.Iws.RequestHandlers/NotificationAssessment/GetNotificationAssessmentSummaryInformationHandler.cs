namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Core.NotificationAssessment;
    using DataAccess;

    internal class GetNotificationAssessmentSummaryInformationHandler : IRequestHandler<GetNotificationAssessmentSummaryInformation, NotificationAssessmentSummaryInformationData>
    {
        private readonly IwsContext context;

        public GetNotificationAssessmentSummaryInformationHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<NotificationAssessmentSummaryInformationData> HandleAsync(GetNotificationAssessmentSummaryInformation message)
        {
            return await context.Database.SqlQuery<NotificationAssessmentSummaryInformationData>(
                @"SELECT
                    N.[Id],
                    N.[NotificationNumber] AS [Number],
                    N.[CompetentAuthority],
                    NA.[Status],
                    LA.[Name] AS [Area]
                FROM
                    [Notification].[Notification] N
                    INNER JOIN [Notification].[NotificationAssessment] NA ON NA.[NotificationApplicationId] = N.[Id]
                    LEFT JOIN [Notification].[Consultation] C 
                        INNER JOIN [Lookup].[LocalArea] LA ON LA.[Id] = C.[LocalAreaId]
                    ON C.[NotificationId] = N.[Id]
                WHERE
                    N.[Id] = @notificationId",
                new SqlParameter("@notificationId", message.Id)).SingleAsync();
        }
    }
}
