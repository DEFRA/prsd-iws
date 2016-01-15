namespace EA.Iws.RequestHandlers.Notification
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class ResubmitNotificationHandler : IRequestHandler<ResubmitNotification, bool>
    {
        private readonly IwsContext context;
        private readonly INotificationAssessmentRepository repository;

        public ResubmitNotificationHandler(INotificationAssessmentRepository repository,
            IwsContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        public async Task<bool> HandleAsync(ResubmitNotification message)
        {
            var assessment = await repository.GetByNotificationId(message.NotificationId);

            assessment.Resubmit();

            await context.SaveChangesAsync();

            return true;
        }
    }
}