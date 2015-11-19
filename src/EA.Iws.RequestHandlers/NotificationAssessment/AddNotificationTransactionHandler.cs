namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.NotificationAssessment;
    using DataAccess;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class AddNotificationTransactionHandler : IRequestHandler<AddNotificationTransaction, bool>
    {
        private readonly IwsContext context;
        private readonly INotificationTransactionRepository repository;
        private readonly INotificationChargeCalculator chargeCalculator;
        private readonly NotificationTransactionCalculator transactionCalculator;

        public AddNotificationTransactionHandler(IwsContext context, 
            INotificationTransactionRepository repository, 
            INotificationChargeCalculator chargeCalculator, 
            NotificationTransactionCalculator transactionCalculator)
        {
            this.context = context;
            this.repository = repository;
            this.chargeCalculator = chargeCalculator;
            this.transactionCalculator = transactionCalculator;
        }

        public async Task<bool> HandleAsync(AddNotificationTransaction message)
        {
            repository.Add(message.Data);
            
            if (message.Data.Credit > 0 && await PaymentIsFullyReceived(message.Data))
            {
                var assessment = await context.NotificationAssessments.SingleAsync(p => p.NotificationApplicationId == message.Data.NotificationId);

                if (assessment.Dates.PaymentReceivedDate == null)
                {
                    assessment.PaymentReceived(message.Data.Date); 
                }
            }

            await context.SaveChangesAsync();

            return true;
        }

        private async Task<bool> PaymentIsFullyReceived(NotificationTransactionData data)
        {
            var transactions = await repository.GetTransactions(data.NotificationId);
            var totalBillable = await GetTotalBillable(data.NotificationId);

            var totalPaid = transactionCalculator.TotalCredits(transactions) 
                - transactionCalculator.TotalDebits(transactions)
                + data.Credit.GetValueOrDefault()
                - data.Debit.GetValueOrDefault();

            return totalBillable <= totalPaid;
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
