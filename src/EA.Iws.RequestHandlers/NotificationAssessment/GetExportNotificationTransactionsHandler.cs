namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Shared;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class GetExportNotificationTransactionsHandler : IRequestHandler<GetExportNotificationTransactions, IList<TransactionRecordData>>
    {
        private readonly INotificationTransactionRepository repository;
        private readonly IMapper mapper;

        public GetExportNotificationTransactionsHandler(INotificationTransactionRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        
        public async Task<IList<TransactionRecordData>> HandleAsync(GetExportNotificationTransactions message)
        {
            var transactions = await repository.GetTransactions(message.NotificationId);

            return transactions.Select(t => mapper.Map<TransactionRecordData>(t)).ToList();
        }
    }
}
