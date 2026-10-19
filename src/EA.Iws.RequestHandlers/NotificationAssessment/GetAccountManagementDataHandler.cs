namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.NotificationAssessment;
    using Core.Shared;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class GetAccountManagementDataHandler : IRequestHandler<GetAccountManagementData, AccountManagementData>
    {
        private readonly INotificationTransactionRepository repository;
        private readonly IMap<IList<NotificationTransaction>, AccountManagementData> accountManagementMap;
        private readonly INotificationChargeCalculator chargeCalculator;
        private readonly INotificationTransactionCalculator transactionCalculator;
        private readonly IGovUkPaySessionRepository govUkPaySessionRepository;

        public GetAccountManagementDataHandler(INotificationTransactionRepository repository,
            IMap<IList<NotificationTransaction>, AccountManagementData> accountManagementMap,
            INotificationChargeCalculator chargeCalculator,
            INotificationTransactionCalculator transactionCalculator,
            IGovUkPaySessionRepository govUkPaySessionRepository)
        {
            this.repository = repository;
            this.accountManagementMap = accountManagementMap;
            this.chargeCalculator = chargeCalculator;
            this.transactionCalculator = transactionCalculator;
            this.govUkPaySessionRepository = govUkPaySessionRepository;
        }

        public async Task<AccountManagementData> HandleAsync(GetAccountManagementData message)
        {
            var transactions = await repository.GetTransactions(message.NotificationId);

            var accountManagementData = accountManagementMap.Map(transactions);

            var failedSessions = await govUkPaySessionRepository.GetFailedByNotificationId(message.NotificationId);

            if (failedSessions != null)
            {
                foreach (var session in failedSessions)
                {
                    accountManagementData.PaymentHistory.Add(new TransactionRecordData
                    {
                        Transaction = TransactionType.Failed,
                        Date = session.UpdatedDate.GetValueOrDefault(session.CreatedDate),
                        Amount = session.Amount,
                        Type = PaymentMethod.GovPay,
                        ReceiptNumber = session.PaymentReference,
                        TransactionId = session.Id
                    });
                }
            }

            accountManagementData.PaymentHistory = accountManagementData.PaymentHistory
                .OrderBy(t => t.Date)
                .ToList();

            var totalBillable = await chargeCalculator.GetValue(message.NotificationId);

            accountManagementData.TotalBillable = totalBillable;
            accountManagementData.Balance = await transactionCalculator.TotalPaid(message.NotificationId);

            return accountManagementData;
        }
    }
}
