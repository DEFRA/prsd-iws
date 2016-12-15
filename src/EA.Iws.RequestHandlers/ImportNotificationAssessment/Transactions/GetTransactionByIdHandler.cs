namespace EA.Iws.RequestHandlers.ImportNotificationAssessment.Transactions
{
    using System.Threading.Tasks;
    using Core.NotificationAssessment;
    using Core.Shared;
    using DataAccess;
    using Domain.ImportNotificationAssessment.Transactions;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment.Transactions;

    internal class GetTransactionByIdHandler : IRequestHandler<GetTransactionById, TransactionRecordData>
    {
        private readonly IImportNotificationTransactionRepository repository;

        public GetTransactionByIdHandler(IImportNotificationTransactionRepository repository)
        {
            this.repository = repository;
        }

        public async Task<TransactionRecordData> HandleAsync(GetTransactionById message)
        {
            var transaction = await repository.GetById(message.TransactionId);

            return new TransactionRecordData
            {
                Transaction = transaction.Credit.HasValue ? TransactionType.Payment : TransactionType.Refund,
                Date = transaction.Date,
                Amount = transaction.Credit.HasValue ? transaction.Credit.Value : transaction.Debit.GetValueOrDefault(0),
                Type = transaction.PaymentMethod,
                ReceiptNumber = transaction.ReceiptNumber,
                Comments = transaction.Comments,
                TransactionId = transaction.Id
            };
        }
    }
}
