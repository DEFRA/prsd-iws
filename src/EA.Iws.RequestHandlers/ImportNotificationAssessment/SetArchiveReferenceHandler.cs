namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportNotification;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;

    internal class SetArchiveReferenceHandler : IRequestHandler<SetArchiveReference, Unit>
    {
        private readonly ImportNotificationContext context;
        private readonly IImportNotificationAssessmentRepository repository;

        public SetArchiveReferenceHandler(ImportNotificationContext context,
            IImportNotificationAssessmentRepository repository)
        {
            this.context = context;
            this.repository = repository;
        }

        public async Task<Unit> HandleAsync(SetArchiveReference message)
        {
            var assessment = await repository.GetByNotification(message.NotificationId);
            assessment.SetArchiveReference(message.ArchiveReference);
            await context.SaveChangesAsync();
            return Unit.Value;
        }
    }
}