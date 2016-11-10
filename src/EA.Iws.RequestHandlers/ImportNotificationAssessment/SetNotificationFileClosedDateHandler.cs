namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportNotification;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;

    internal class SetNotificationFileClosedDateHandler : IRequestHandler<SetNotificationFileClosedDate, Unit>
    {
        private readonly ImportNotificationContext context;
        private readonly IImportNotificationAssessmentRepository repository;

        public SetNotificationFileClosedDateHandler(IImportNotificationAssessmentRepository repository,
            ImportNotificationContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        public async Task<Unit> HandleAsync(SetNotificationFileClosedDate message)
        {
            var assessment = await repository.GetByNotification(message.ImportNotificationId);
            assessment.MarkFileClosed(message.Date);
            await context.SaveChangesAsync();
            return Unit.Value;
        }
    }
}