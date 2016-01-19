namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportNotification;
    using Prsd.Core;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;

    public class WithdrawConsentForImportNotificationHandler : IRequestHandler<WithdrawConsentForImportNotification, bool>
    {
        private readonly IImportNotificationAssessmentRepository repository;
        private readonly ImportNotificationContext context;
        
        public WithdrawConsentForImportNotificationHandler(IImportNotificationAssessmentRepository repository, ImportNotificationContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        public async Task<bool> HandleAsync(WithdrawConsentForImportNotification message)
        {
            var assessment = await repository.GetByNotification(message.Id);

            assessment.WithdrawConsent(SystemTime.UtcNow, message.ReasonsForConsentWithdrawal);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
