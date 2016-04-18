namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Threading.Tasks;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class GetChargeDueHandler : IRequestHandler<GetChargeDue, decimal>
    {
        private readonly INotificationTransactionCalculator transactionCalculator;

        public GetChargeDueHandler(INotificationTransactionCalculator transactionCalculator)
        {
            this.transactionCalculator = transactionCalculator;
        }

        public async Task<decimal> HandleAsync(GetChargeDue message)
        {
            return await transactionCalculator.Balance(message.NotificationId);
        }
    }
}