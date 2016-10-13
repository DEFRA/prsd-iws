namespace EA.Iws.RequestHandlers.ImportNotificationAssessment.Transactions
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportNotificationAssessment.Transactions;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment.Transactions;

    internal class AddNotificationRefundHandler : IRequestHandler<AddNotificationRefund, bool>
    {
        private readonly ImportNotificationContext context;
        private readonly ImportRefundTransaction importRefundTransaction;

        public AddNotificationRefundHandler(ImportRefundTransaction importRefundTransaction,
            ImportNotificationContext context)
        {
            this.importRefundTransaction = importRefundTransaction;
            this.context = context;
        }

        public async Task<bool> HandleAsync(AddNotificationRefund message)
        {
            await
                importRefundTransaction.Save(message.ImportNotificationId, message.Date, message.Amount,
                    message.Comments);

            await context.SaveChangesAsync();

            return true;
        }
    }
}