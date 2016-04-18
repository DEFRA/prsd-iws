namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using Core.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;
    using System.Threading.Tasks;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mapper;

    public class GetNotificationAssessmentDecisionDataHandler : IRequestHandler<GetNotificationAssessmentDecisionData, NotificationAssessmentDecisionData>
    {
        private readonly INotificationAssessmentRepository notificationAssessmentRepository;
        private readonly IMap<NotificationAssessment, NotificationAssessmentDecisionData> assessmentDecisionMap;

        public GetNotificationAssessmentDecisionDataHandler(INotificationAssessmentRepository notificationAssessmentRepository,
            IMap<NotificationAssessment, NotificationAssessmentDecisionData> assessmentDecisionMap)
        {
            this.notificationAssessmentRepository = notificationAssessmentRepository;
            this.assessmentDecisionMap = assessmentDecisionMap;
        }

        public async Task<NotificationAssessmentDecisionData> HandleAsync(GetNotificationAssessmentDecisionData message)
        {
            var notificationAssessment =
                await notificationAssessmentRepository.GetByNotificationId(message.NotificationId);

            return assessmentDecisionMap.Map(notificationAssessment);
        }
    }
}
