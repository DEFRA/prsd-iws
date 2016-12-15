namespace EA.Iws.RequestHandlers.ImportNotificationAssessment.Transactions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Shared;
    using Domain.ImportNotificationAssessment.Transactions;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment.Transactions;

    internal class GetImportNotificationTransactionsHandler : IRequestHandler<GetImportNotificationTransactions, IList<TransactionRecordData>>
    {
        private readonly IImportNotificationTransactionRepository repository;
        private readonly IMapper mapper;

        public GetImportNotificationTransactionsHandler(IImportNotificationTransactionRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<IList<TransactionRecordData>> HandleAsync(GetImportNotificationTransactions message)
        {
            var transactions = await repository.GetTransactions(message.NotificationId);

            return transactions.Select(t => mapper.Map<TransactionRecordData>(t)).ToList();
        }
    }
}
