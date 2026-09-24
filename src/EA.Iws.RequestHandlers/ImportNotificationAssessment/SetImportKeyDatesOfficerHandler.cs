namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using System.Threading.Tasks;
    using Domain.ImportNotification;
    using Domain.ImportNotificationAssessment;
    using Domain.ImportNotificationAssessment.Decision;
    using EA.Iws.Requests.ImportNotificationAssessment;
    using Prsd.Core.Mediator;

    internal class SetImportKeyDatesOfficerHandler : IRequestHandler<SetImportKeyDatesOfficer, Unit>
    {
        private readonly IKeyDatesRepository repository;
        private readonly IImportNotificationAssessmentRepository assessmentRepository;
        private readonly DecisionRequiredBy decisionRequiredBy;

        public SetImportKeyDatesOfficerHandler(IKeyDatesRepository repository,
            IImportNotificationAssessmentRepository assessmentRepository,
            DecisionRequiredBy decisionRequiredBy)
        {
            this.repository = repository;
            this.assessmentRepository = assessmentRepository;
            this.decisionRequiredBy = decisionRequiredBy;
        }

        public async Task<Unit> HandleAsync(SetImportKeyDatesOfficer message)
        {
            var assessment = await assessmentRepository.GetByNotification(message.NotificationId);

            await repository.SetNameOfOfficerForNotification(assessment.Id, message.NameOfOfficer);

            return Unit.Value;
        }
    }
}