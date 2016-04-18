namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class WithdrawNotificationApplicationHandler : IRequestHandler<WithdrawNotificationApplication, bool>
    {
        private readonly INotificationAssessmentRepository notificationAssessmentRepository;
        private readonly IwsContext context;

        public WithdrawNotificationApplicationHandler(INotificationAssessmentRepository notificationAssessmentRepository, IwsContext context)
        {
            this.notificationAssessmentRepository = notificationAssessmentRepository;
            this.context = context;
        }

        public async Task<bool> HandleAsync(WithdrawNotificationApplication message)
        {
            var assessment = await notificationAssessmentRepository.GetByNotificationId(message.Id);

            assessment.Withdraw(message.Date, message.Reason);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
