namespace EA.Iws.RequestHandlers.Mappings.NotificationAssessment
{
    using System.Collections.Generic;
    using Core.NotificationAssessment;
    using Core.Shared;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mapper;

    internal class AccountManageDataMap : IMap<IList<NotificationTransaction>, AccountManagementData>
    {
        public AccountManagementData Map(IList<NotificationTransaction> source)
        {
            AccountManagementData data = new AccountManagementData
            {
                PaymentHistory = new List<TransactionRecordData>()
            };

            if (source != null)
            {
                foreach (var item in source)
                {
                    var d = new TransactionRecordData
                    {
                        Comments = item.Comments,
                        ReceiptNumber = item.ReceiptNumber,
                        Date = item.Date,
                        Type = item.PaymentMethod,
                        Transaction = IsCredit(item) ? TransactionType.Payment : TransactionType.Refund,
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
