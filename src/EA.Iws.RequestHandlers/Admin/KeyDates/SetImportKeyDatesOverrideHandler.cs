namespace EA.Iws.RequestHandlers.Admin.KeyDates
{
    using System.Threading.Tasks;
    using Domain.ImportNotification;
    using Domain.ImportNotificationAssessment;
    using Domain.ImportNotificationAssessment.Decision;
    using Prsd.Core.Mediator;
    using Requests.Admin.KeyDates;

    internal class SetimportKeyDatesOverrideHandler : IRequestHandler<SetImportKeyDatesOverride, Unit>
    {
        private readonly IKeyDatesOverrideRepository repository;
        private readonly IImportNotificationAssessmentRepository assessmentRepository;
        private readonly DecisionRequiredBy decisionRequiredBy;

        public SetimportKeyDatesOverrideHandler(IKeyDatesOverrideRepository repository,
            IImportNotificationAssessmentRepository assessmentRepository,
            DecisionRequiredBy decisionRequiredBy)
        {
            this.repository = repository;
            this.assessmentRepository = assessmentRepository;
            this.decisionRequiredBy = decisionRequiredBy;
        }

        public async Task<Unit> HandleAsync(SetImportKeyDatesOverride message)
        {
            var assessment = await assessmentRepository.GetByNotification(message.Data.NotificationId);
            var currentDecisionRequiredByDate = await decisionRequiredBy.GetDecisionRequiredByDate(assessment);

            if (currentDecisionRequiredByDate != message.Data.DecisionRequiredByDate)
            {
                await repository.SetDecisionRequiredByDateForNotification(assessment.Id, message.Data.DecisionRequiredByDate);
            }

            await repository.SetKeyDatesForNotification(message.Data);

            return Unit.Value;
        }
    }
}