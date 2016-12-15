namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Threading.Tasks;
    using Core.NotificationAssessment;
    using Core.Shared;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class GetTransactionByIdHandler : IRequestHandler<GetTransactionById, TransactionRecordData>
    {
        private readonly INotificationTransactionRepository repository;

        public GetTransactionByIdHandler(INotificationTransactionRepository repository)
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
