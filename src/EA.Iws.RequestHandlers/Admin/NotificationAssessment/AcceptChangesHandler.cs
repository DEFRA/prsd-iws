namespace EA.Iws.RequestHandlers.Admin.NotificationAssessment
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.Admin.NotificationAssessment;

    internal class AcceptChangesHandler : IRequestHandler<AcceptChanges, bool>
    {
        private readonly IwsContext context;
        private readonly INotificationAssessmentRepository repository;

        public AcceptChangesHandler(INotificationAssessmentRepository repository,
            IwsContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        public async Task<bool> HandleAsync(AcceptChanges message)
        {
            var assessment = await repository.GetByNotificationId(message.NotificationId);

            assessment.AcceptChanges();

            await context.SaveChangesAsync();

            return true;
        }
    }
}