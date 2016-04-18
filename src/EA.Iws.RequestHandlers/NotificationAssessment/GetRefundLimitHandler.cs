namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Threading.Tasks;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class GetRefundLimitHandler : IRequestHandler<GetRefundLimit, decimal>
    {
        private readonly INotificationTransactionCalculator transactionCalculator;

        public GetRefundLimitHandler(INotificationTransactionCalculator transactionCalculator)
        {
            this.transactionCalculator = transactionCalculator;
        }

        public async Task<decimal> HandleAsync(GetRefundLimit message)
        {
            return await transactionCalculator.RefundLimit(message.NotificationId);
        }
    }
}
