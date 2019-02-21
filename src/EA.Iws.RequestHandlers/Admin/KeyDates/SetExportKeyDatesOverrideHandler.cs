namespace EA.Iws.RequestHandlers.Admin.KeyDates
{
    using System;
    using System.Threading.Tasks;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.Admin.KeyDates;

    internal class SetExportKeyDatesOverrideHandler : IRequestHandler<SetExportKeyDatesOverride, Unit>
    {
        private readonly IKeyDatesOverrideRepository repository;
        private readonly INotificationAssessmentRepository assessmentRepository;
        private readonly INotificationApplicationRepository applicationRepository;
        private readonly DecisionRequiredBy decisionRequiredBy;

        public SetExportKeyDatesOverrideHandler(IKeyDatesOverrideRepository repository,
            INotificationAssessmentRepository assessmentRepository,
            INotificationApplicationRepository applicationRepository,
            DecisionRequiredBy decisionRequiredBy)
        {
            this.repository = repository;
            this.assessmentRepository = assessmentRepository;
            this.applicationRepository = applicationRepository;
            this.decisionRequiredBy = decisionRequiredBy;
        }

        public async Task<Unit> HandleAsync(SetExportKeyDatesOverride message)
        {
            var assessment = await assessmentRepository.GetByNotificationId(message.Data.NotificationId);
            var notification = await applicationRepository.GetById(message.Data.NotificationId);
            var currentDecisionRequiredByDate = await decisionRequiredBy.GetDecisionRequiredByDate(notification, assessment);

            if (currentDecisionRequiredByDate != message.Data.DecisionRequiredByDate)
            {
                await repository.SetDecisionRequiredByDateForNotification(assessment.Id, message.Data.DecisionRequiredByDate);
            }

            await repository.SetKeyDatesForNotification(message.Data);

            return Unit.Value;
        }
    }
}