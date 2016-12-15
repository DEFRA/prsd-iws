namespace EA.Iws.RequestHandlers.Mappings.NotificationAssessment
{
    using Core.NotificationAssessment;
    using Core.Shared;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mapper;

    internal class NotificationTransactionMap : IMap<NotificationTransaction, TransactionRecordData>
    {
        public TransactionRecordData Map(NotificationTransaction source)
        {
            return new TransactionRecordData
            {
                Amount = source.Credit.HasValue ? source.Credit.Value : source.Debit.GetValueOrDefault(0),
                Date = source.Date,
                ReceiptNumber = source.ReceiptNumber,
                Comments = source.Comments,
                Type = source.PaymentMethod,
                Transaction = source.Credit.HasValue ? TransactionType.Payment : TransactionType.Refund,
                TransactionId = source.Id
            };
        }
    }
}
