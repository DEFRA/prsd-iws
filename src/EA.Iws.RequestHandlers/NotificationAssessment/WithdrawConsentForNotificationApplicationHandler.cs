namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationAssessment;
    using Prsd.Core;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class WithdrawConsentForNotificationApplicationHandler : IRequestHandler<WithdrawConsentForNotificationApplication, bool>
    {
        private readonly INotificationAssessmentRepository notificationAssessmentRepository;
        private readonly IwsContext context;

        public WithdrawConsentForNotificationApplicationHandler(INotificationAssessmentRepository notificationAssessmentRepository, 
            IwsContext context)
        {
            this.notificationAssessmentRepository = notificationAssessmentRepository;
            this.context = context;
        }

        public async Task<bool> HandleAsync(WithdrawConsentForNotificationApplication message)
        {
            var assessment = await notificationAssessmentRepository.GetByNotificationId(message.Id);

            assessment.WithdrawConsent(SystemTime.UtcNow, message.ReasonsForConsentWithdrawal);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
