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
        private readonly NotificationChargeCalculator chargeCalculator;
        private readonly NotificationTransactionCalculator transactionCalculator;
        private readonly IwsContext context;

        public GetAccountManagementDataHandler(INotificationTransactionRepository repository,
            IMap<IList<NotificationTransaction>, AccountManagementData> accountManagementMap,
            NotificationChargeCalculator chargeCalculator,
            NotificationTransactionCalculator transactionCalculator,
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

            var totalBillable = await GetTotalBillable(message.NotificationId);
            var credits = transactionCalculator.TotalCredits(transactions);
            var debits = transactionCalculator.TotalDebits(transactions);

            accountManagementData.TotalBillable = totalBillable;
            accountManagementData.Balance = totalBillable - credits + debits;

            return accountManagementData;
        }

        private async Task<decimal> GetTotalBillable(Guid id)
        {
            var notification = await context.NotificationApplications.Where(n => n.Id == id).SingleAsync();
            var pricingStructures = await context.PricingStructures.ToArrayAsync();
            var shipmentInfo = await context.ShipmentInfos.Where(s => s.NotificationId == id).FirstAsync();

            return chargeCalculator.GetValue(pricingStructures, notification, shipmentInfo);
        }
    }
}
