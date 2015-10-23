namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Threading.Tasks;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class GetNotificationNumberForAssessmentHandler : IRequestHandler<GetNotificationNumberForAssessment, string>
    {
        private readonly INotificationAssessmentRepository notificationAssessmentRepository;

        public GetNotificationNumberForAssessmentHandler(INotificationAssessmentRepository notificationAssessmentRepository)
        {
            this.notificationAssessmentRepository = notificationAssessmentRepository;
        }

        public async Task<string> HandleAsync(GetNotificationNumberForAssessment message)
        {
            return await notificationAssessmentRepository.GetNumberForAssessment(message.NotificationAssessmentId);
        }
    }
}
