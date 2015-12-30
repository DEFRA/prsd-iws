namespace EA.Iws.Domain.ImportNotificationAssessment.Transactions
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IImportNotificationTransactionCalculator
    {
        Task<decimal> Balance(Guid importNotificationId);

        decimal TotalDebits(IEnumerable<ImportNotificationTransaction> transactions);

        decimal TotalCredits(IEnumerable<ImportNotificationTransaction> transactions);

        Task<bool> PaymentIsNowFullyReceived(Guid importNotificationId, decimal credit);
    }
}