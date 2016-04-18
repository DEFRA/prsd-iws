namespace EA.Iws.RequestHandlers.ImportNotificationAssessment.Transactions
{
    using System.Threading.Tasks;
    using Domain.ImportNotificationAssessment.Transactions;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment.Transactions;

    internal class GetChargeDueHandler : IRequestHandler<GetChargeDue, decimal>
    {
        private readonly IImportNotificationTransactionCalculator transactionCalculator;

        public GetChargeDueHandler(IImportNotificationTransactionCalculator transactionCalculator)
        {
            this.transactionCalculator = transactionCalculator;
        }

        public async Task<decimal> HandleAsync(GetChargeDue message)
        {
            return await transactionCalculator.Balance(message.ImportNotificationId);
        }
    }
}