namespace EA.Iws.RequestHandlers.ImportNotificationAssessment.Transactions
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportNotificationAssessment.Transactions;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment.Transactions;

    internal class AddNotificationPaymentHandler : IRequestHandler<AddNotificationPayment, bool>
    {
        private readonly ImportPaymentTransaction importPaymentTransaction;
        private readonly ImportNotificationContext context;

        public AddNotificationPaymentHandler(ImportPaymentTransaction importPaymentTransaction, 
            ImportNotificationContext context)
        {
            this.importPaymentTransaction = importPaymentTransaction;
            this.context = context;
        }

        public async Task<bool> HandleAsync(AddNotificationPayment message)
        {
            await
                importPaymentTransaction.Save(message.ImportNotificationId, message.Date, message.Amount,
                    message.PaymentMethod, message.ReceiptNumber, message.Comments);

            await context.SaveChangesAsync();

            return false;
        }
    }
}
