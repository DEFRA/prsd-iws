namespace EA.Iws.RequestHandlers.Admin.NotificationAssessment
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.Admin.NotificationAssessment;

    internal class RejectChangesHandler : IRequestHandler<RejectChanges, bool>
    {
        private readonly IwsContext context;
        private readonly INotificationAssessmentRepository repository;

        public RejectChangesHandler(INotificationAssessmentRepository repository,
            IwsContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        public async Task<bool> HandleAsync(RejectChanges message)
        {
            var assessment = await repository.GetByNotificationId(message.NotificationId);

            assessment.RejectChanges();

            await context.SaveChangesAsync();

            return true;
        }
    }
}