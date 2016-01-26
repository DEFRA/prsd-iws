namespace EA.Iws.RequestHandlers.ImportNotificationAssessment.Transactions
{
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ImportNotificationAssessment.Transactions;
    using Core.Shared;
    using Domain.ImportNotificationAssessment;
    using Domain.ImportNotificationAssessment.Transactions;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment.Transactions;

    internal class GetImportNotificationAccountOverviewHandler : IRequestHandler<GetImportNotificationAccountOverview, AccountOverviewData>
    {
        private readonly IImportNotificationChargeCalculator chargeCalculator;
        private readonly IImportNotificationTransactionCalculator transactionCalculator;
        private readonly IImportNotificationTransactionRepository transactionRepository;
        private readonly IMapper mapper;

        public GetImportNotificationAccountOverviewHandler(IImportNotificationChargeCalculator chargeCalculator, 
            IImportNotificationTransactionCalculator transactionCalculator,
            IImportNotificationTransactionRepository transactionRepository,
            IMapper mapper)
        {
            this.chargeCalculator = chargeCalculator;
            this.transactionCalculator = transactionCalculator;
            this.transactionRepository = transactionRepository;
            this.mapper = mapper;
        }

        public async Task<AccountOverviewData> HandleAsync(GetImportNotificationAccountOverview message)
        {
            var charge = await chargeCalculator.GetValue(message.ImportNotificationId);

            var transactions = await transactionRepository.GetTransactions(message.ImportNotificationId);

            var totalPaid = await transactionCalculator.TotalPaid(message.ImportNotificationId);

            return new AccountOverviewData
            {
                TotalCharge = charge,
                TotalPaid = totalPaid,
                Transactions = transactions.Select(t => mapper.Map<TransactionRecordData>(t)).ToArray()
            };
        }
    }
}
