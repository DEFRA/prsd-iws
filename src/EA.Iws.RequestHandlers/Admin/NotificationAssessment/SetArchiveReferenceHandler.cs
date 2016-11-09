namespace EA.Iws.RequestHandlers.Admin.NotificationAssessment
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.Admin.NotificationAssessment;

    internal class SetArchiveReferenceHandler : IRequestHandler<SetArchiveReference, Unit>
    {
        private readonly IwsContext context;
        private readonly INotificationAssessmentRepository repository;

        public SetArchiveReferenceHandler(IwsContext context, INotificationAssessmentRepository repository)
        {
            this.context = context;
            this.repository = repository;
        }

        public async Task<Unit> HandleAsync(SetArchiveReference message)
        {
            var assessment = await repository.GetByNotificationId(message.NotificationId);
            assessment.SetArchiveReference(message.ArchiveReference);
            await context.SaveChangesAsync();
            return Unit.Value;
        }
    }
}