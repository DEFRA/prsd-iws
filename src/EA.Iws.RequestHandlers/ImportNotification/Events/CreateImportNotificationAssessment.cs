namespace EA.Iws.RequestHandlers.ImportNotification.Events
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportNotification;
    using Domain.ImportNotificationAssessment;
    using Prsd.Core.Domain;

    internal class CreateImportNotificationAssessment : IEventHandler<ImportNotificationCreatedEvent>
    {
        private readonly IImportNotificationAssessmentRepository assessmentRepository;
        private readonly ImportNotificationContext context;

        public CreateImportNotificationAssessment(IImportNotificationAssessmentRepository assessmentRepository,
            ImportNotificationContext context)
        {
            this.assessmentRepository = assessmentRepository;
            this.context = context;
        }

        public async Task HandleAsync(ImportNotificationCreatedEvent @event)
        {
            assessmentRepository.Add(new ImportNotificationAssessment(@event.Notification.Id));

            await context.SaveChangesAsync();
        }
    }
}
