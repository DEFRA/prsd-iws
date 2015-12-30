namespace EA.Iws.RequestHandlers.Mappings.ImportNotificationAssessment
{
    using Core.NotificationAssessment;
    using Core.Shared;
    using Domain.ImportNotificationAssessment.Transactions;
    using Prsd.Core.Mapper;

    internal class ImportNotificationTransactionMap : IMap<ImportNotificationTransaction, TransactionRecordData>
    {
        public TransactionRecordData Map(ImportNotificationTransaction source)
        {
            return new TransactionRecordData
            {
                Amount = source.Credit.HasValue ? source.Credit.Value : source.Debit.GetValueOrDefault(0),
                Date = source.Date,
                ReceiptNumber = source.ReceiptNumber,
                Comments = source.Comments,
                Type = source.PaymentMethod,
                Transaction = source.Credit.HasValue ? TransactionType.Payment : TransactionType.Refund
            };
        }
    }
}
