namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using DataAccess;
    using Domain.NotificationAssessment;
    using Prsd.Core;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;
    using System.Threading.Tasks;

    internal class ObjectNotificationApplicationHandler : IRequestHandler<ObjectNotificationApplication, bool>
    {
        private readonly INotificationAssessmentRepository notificationAssessmentRepository;
        private readonly IwsContext context;

        public ObjectNotificationApplicationHandler(INotificationAssessmentRepository notificationAssessmentRepository,
            IwsContext context)
        {
            this.notificationAssessmentRepository = notificationAssessmentRepository;
            this.context = context;
        }

        public async Task<bool> HandleAsync(ObjectNotificationApplication message)
        {
            var assessment = await notificationAssessmentRepository.GetByNotificationId(message.Id);

            assessment.Object(message.Date, message.Reason);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
