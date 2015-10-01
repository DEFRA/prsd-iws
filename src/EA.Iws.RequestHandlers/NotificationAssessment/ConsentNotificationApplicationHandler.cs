namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationAssessment;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class ConsentNotificationApplicationHandler : IRequestHandler<ConsentNotificationApplication, bool>
    {
        private readonly INotificationAssessmentRepository notificationAssessmentRepository;
        private readonly IwsContext context;
        private readonly IUserContext userContext;

        public ConsentNotificationApplicationHandler(INotificationAssessmentRepository notificationAssessmentRepository,
            IwsContext context,
            IUserContext userContext)
        {
            this.notificationAssessmentRepository = notificationAssessmentRepository;
            this.context = context;
            this.userContext = userContext;
        }

        public async Task<bool> HandleAsync(ConsentNotificationApplication message)
        {
            var assessment = await notificationAssessmentRepository.GetByNotificationId(message.NotificationId);

            assessment.RecordConsent(new Consent(message.ConsentRange, message.ConsentConditions, userContext.UserId));

            await context.SaveChangesAsync();

            return true;
        }
    }
}
