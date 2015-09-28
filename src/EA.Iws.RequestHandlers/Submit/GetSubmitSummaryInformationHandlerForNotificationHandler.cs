namespace EA.Iws.RequestHandlers.Submit
{
    using System.Threading.Tasks;
    using Core.Notification;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.Submit;

    internal class GetSubmitSummaryInformationHandlerForNotificationHandler : IRequestHandler<GetSubmitSummaryInformationForNotification, SubmitSummaryData>
    {
        private readonly INotificationApplicationRepository notificationApplicationRepository;
        private readonly INotificationAssessmentRepository notificationAssessmentRepository;

        public GetSubmitSummaryInformationHandlerForNotificationHandler(INotificationApplicationRepository notificationApplicationRepository,
            INotificationAssessmentRepository notificationAssessmentRepository)
        {
            this.notificationApplicationRepository = notificationApplicationRepository;
            this.notificationAssessmentRepository = notificationAssessmentRepository;
        }

        public async Task<SubmitSummaryData> HandleAsync(GetSubmitSummaryInformationForNotification message)
        {
            var notification = await notificationApplicationRepository.GetById(message.Id);

            var assessment =
                await notificationAssessmentRepository.GetByNotificationId(message.Id);

            var competentAuthority = notification.CompetentAuthority.AsCompetentAuthority();
            
            return new SubmitSummaryData
            {
                CompetentAuthority = competentAuthority,
                CreatedDate = notification.CreatedDate,
                NotificationNumber = notification.NotificationNumber,
                Status = assessment.Status
            };
        }
    }
}
