namespace EA.Iws.RequestHandlers.Admin.KeyDates
{
    using System.Threading.Tasks;
    using Core.Admin.KeyDates;
    using Domain.ImportNotification;
    using Domain.ImportNotificationAssessment;
    using Domain.ImportNotificationAssessment.Decision;
    using Prsd.Core.Mediator;
    using Requests.Admin.KeyDates;

    public class GetImportKeyDatesOverrideDataHandler :
        IRequestHandler<GetImportKeyDatesOverrideData, KeyDatesOverrideData>
    {
        private readonly IKeyDatesOverrideRepository repository;
        private readonly IImportNotificationAssessmentRepository assessmentRepository;
        private readonly DecisionRequiredBy decisionRequiredBy;

        public GetImportKeyDatesOverrideDataHandler(IKeyDatesOverrideRepository repository,
            IImportNotificationAssessmentRepository assessmentRepository,
            DecisionRequiredBy decisionRequiredBy)
        {
            this.repository = repository;
            this.assessmentRepository = assessmentRepository;
            this.decisionRequiredBy = decisionRequiredBy;
        }

        public async Task<KeyDatesOverrideData> HandleAsync(GetImportKeyDatesOverrideData message)
        {
            var assessment = await assessmentRepository.GetByNotification(message.NotificationId);
            var keyDates = await repository.GetKeyDatesForNotification(message.NotificationId);

            keyDates.DecisionRequiredByDate = await decisionRequiredBy.GetDecisionRequiredByDate(assessment);

            return keyDates;
        }
    }
}