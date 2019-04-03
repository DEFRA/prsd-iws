namespace EA.Iws.RequestHandlers.Admin.KeyDates
{
    using System.Threading.Tasks;
    using Core.Admin.KeyDates;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.Admin.KeyDates;

    public class GetExportKeyDatesOverrideDataHandler :
        IRequestHandler<GetExportKeyDatesOverrideData, KeyDatesOverrideData>
    {
        private readonly IKeyDatesOverrideRepository repository;
        private readonly INotificationAssessmentRepository assessmentRepository;
        private readonly INotificationApplicationRepository applicationRepository;
        private readonly DecisionRequiredBy decisionRequiredBy;

        public GetExportKeyDatesOverrideDataHandler(IKeyDatesOverrideRepository repository,
            INotificationAssessmentRepository assessmentRepository,
            INotificationApplicationRepository applicationRepository,
            DecisionRequiredBy decisionRequiredBy)
        {
            this.repository = repository;
            this.assessmentRepository = assessmentRepository;
            this.applicationRepository = applicationRepository;
            this.decisionRequiredBy = decisionRequiredBy;
        }

        public async Task<KeyDatesOverrideData> HandleAsync(GetExportKeyDatesOverrideData message)
        {
            var assessment = await assessmentRepository.GetByNotificationId(message.NotificationId);
            var notification = await applicationRepository.GetById(message.NotificationId);
            var keyDates = await repository.GetKeyDatesForNotification(message.NotificationId);

            keyDates.DecisionRequiredByDate = await decisionRequiredBy.GetDecisionRequiredByDate(notification, assessment);

            return keyDates;
        }
    }
}