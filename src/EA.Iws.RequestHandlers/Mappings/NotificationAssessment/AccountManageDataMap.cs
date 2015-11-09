namespace EA.Iws.RequestHandlers.Mappings.NotificationAssessment
{
    using System.Collections.Generic;
    using Core.NotificationAssessment;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mapper;

    internal class AccountManageDataMap : IMap<IList<NotificationTransaction>, AccountManagementData>
    {
        public AccountManagementData Map(IList<NotificationTransaction> source)
        {
            AccountManagementData data = new AccountManagementData
            {
                PaymentHistory = new List<PaymentHistoryTableData>()
            };

            if (source != null)
            {
                foreach (var item in source)
                {
                    var d = new PaymentHistoryTableData
                    {
                        Comments = item.Comments,
                        Receipt = item.ReceiptNumber,
                        Date = item.Date,
                        Type = item.PaymentMethod.GetValueOrDefault(),
                        Transaction = IsCredit(item) ? (int)TransactionType.Payment : (int)TransactionType.Refund,
                        Amount = IsCredit(item) ? item.Credit.GetValueOrDefault() : item.Debit.GetValueOrDefault()
                    };

                    data.PaymentHistory.Add(d);
                }
            }

            return data;
        }

        private bool IsCredit(NotificationTransaction transaction)
        {
            return transaction.Credit != null && transaction.Debit == null;
        }
    }
}
