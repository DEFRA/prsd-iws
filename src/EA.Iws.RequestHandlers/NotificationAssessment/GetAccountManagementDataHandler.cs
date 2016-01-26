namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.NotificationAssessment;
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

        public GetAccountManagementDataHandler(INotificationTransactionRepository repository,
            IMap<IList<NotificationTransaction>, AccountManagementData> accountManagementMap,
            INotificationChargeCalculator chargeCalculator,
            INotificationTransactionCalculator transactionCalculator)
        {
            this.repository = repository;
            this.accountManagementMap = accountManagementMap;
            this.chargeCalculator = chargeCalculator;
            this.transactionCalculator = transactionCalculator;
        }

        public async Task<AccountManagementData> HandleAsync(GetAccountManagementData message)
        {
            var transactions = await repository.GetTransactions(message.NotificationId);

            var accountManagementData = accountManagementMap.Map(transactions);

            var totalBillable = await chargeCalculator.GetValue(message.NotificationId);

            accountManagementData.TotalBillable = totalBillable;
            accountManagementData.Balance = await transactionCalculator.TotalPaid(message.NotificationId);

            return accountManagementData;
        }
    }
}
