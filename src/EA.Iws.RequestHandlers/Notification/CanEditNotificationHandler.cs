namespace EA.Iws.RequestHandlers.Notification
{
    using System.Threading.Tasks;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class CanEditNotificationHandler : IRequestHandler<CanEditNotification, bool>
    {
        private readonly INotificationAssessmentRepository repository;

        public CanEditNotificationHandler(INotificationAssessmentRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> HandleAsync(CanEditNotification message)
        {
            var assessment = await repository.GetByNotificationId(message.NotificationId);

            return assessment.CanEditNotification;
        }
    }
}