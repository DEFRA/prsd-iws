namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Threading.Tasks;
    using Domain.NotificationAssessment;
    using EA.Iws.Requests.Requests.NotificationAssessment;
    using Prsd.Core.Mediator;

    internal class SetImportKeyDatesOfficerHandler : IRequestHandler<SetImportKeyDatesOfficer, Unit>
    {
        private readonly IKeyDatesRepository repository;
        private readonly INotificationAssessmentRepository assessmentRepository;
        private readonly DecisionRequiredBy decisionRequiredBy;

        public SetImportKeyDatesOfficerHandler(IKeyDatesRepository repository,
            INotificationAssessmentRepository assessmentRepository,
            DecisionRequiredBy decisionRequiredBy)
        {
            this.repository = repository;
            this.assessmentRepository = assessmentRepository;
            this.decisionRequiredBy = decisionRequiredBy;
        }

        public async Task<Unit> HandleAsync(SetImportKeyDatesOfficer message)
        {
            var assessment = await assessmentRepository.GetByNotificationId(message.NotificationId);

            await repository.SetNameOfOfficerForNotification(assessment.Id, message.NameOfOfficer);

            return Unit.Value;
        }
    }
}