namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Threading.Tasks;
    using Core.NotificationAssessment;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    public class GetNotificationStatusHandler : IRequestHandler<GetNotificationStatus, NotificationStatus>
    {
        private readonly INotificationAssessmentRepository notificationAssessmentRepository;

        public GetNotificationStatusHandler(INotificationAssessmentRepository notificationAssessmentRepository)
        {
            this.notificationAssessmentRepository = notificationAssessmentRepository;
        }

        public async Task<NotificationStatus> HandleAsync(GetNotificationStatus message)
        {
            return (await notificationAssessmentRepository.GetByNotificationId(message.NotificationId)).Status;
        }
    }
}