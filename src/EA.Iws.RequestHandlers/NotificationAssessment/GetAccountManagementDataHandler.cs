namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.NotificationAssessment;
    using DataAccess;
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
        private readonly IwsContext context;

        public GetAccountManagementDataHandler(INotificationTransactionRepository repository,
            IMap<IList<NotificationTransaction>, AccountManagementData> accountManagementMap,
            INotificationChargeCalculator chargeCalculator,
            INotificationTransactionCalculator transactionCalculator,
            IwsContext context)
        {
            this.repository = repository;
            this.accountManagementMap = accountManagementMap;
            this.chargeCalculator = chargeCalculator;
            this.transactionCalculator = transactionCalculator;
            this.context = context;
        }

        public async Task<AccountManagementData> HandleAsync(GetAccountManagementData message)
        {
            var transactions = await repository.GetTransactions(message.NotificationId);

            var accountManagementData = accountManagementMap.Map(transactions);

            var totalBillable = await chargeCalculator.GetValue(message.NotificationId);
            var credits = transactionCalculator.TotalCredits(transactions);
            var debits = transactionCalculator.TotalDebits(transactions);

            accountManagementData.TotalBillable = totalBillable;
            accountManagementData.Balance = credits - debits;

            return accountManagementData;
        }
    }
}
