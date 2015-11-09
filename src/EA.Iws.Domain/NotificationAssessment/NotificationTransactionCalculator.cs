namespace EA.Iws.Domain.NotificationAssessment
{
    using System.Collections.Generic;
    using System.Linq;

    public class NotificationTransactionCalculator
    {
        public decimal TotalCredits(IList<NotificationTransaction> transactions)
        {
            return transactions.Sum(t => t.Credit).GetValueOrDefault();
        }

        public decimal TotalDebits(IList<NotificationTransaction> transactions)
        {
            return transactions.Sum(t => t.Debit).GetValueOrDefault();
        }
    }
}
