namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;

    internal class WithdrawImportNotificationHandler : IRequestHandler<WithdrawImportNotification, bool>
    {
        private readonly Domain.ImportNotificationAssessment.Decision.WithdrawImportNotification withdrawImportNotification;
        private readonly ImportNotificationContext context;

        public WithdrawImportNotificationHandler(Domain.ImportNotificationAssessment.Decision.WithdrawImportNotification withdrawImportNotification,
            ImportNotificationContext context)
        {
            this.withdrawImportNotification = withdrawImportNotification;
            this.context = context;
        }

        public async Task<bool> HandleAsync(WithdrawImportNotification message)
        {
            var result =
                await withdrawImportNotification.Withdraw(message.ImportNotificationId, message.Date, message.Reasons);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
