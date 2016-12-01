namespace EA.Iws.Domain.FinancialGuarantee
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Prsd.Core.Domain;
    using Prsd.Core.Extensions;

    public class FinancialGuaranteeCollection : Entity
    {
        protected FinancialGuaranteeCollection()
        {
        }

        public FinancialGuaranteeCollection(Guid notificationId)
        {
            NotificationId = notificationId;
            FinancialGuaranteesCollection = new List<FinancialGuarantee>();
        }

        public Guid NotificationId { get; private set; }

        protected virtual ICollection<FinancialGuarantee> FinancialGuaranteesCollection { get; set; }

        public IEnumerable<FinancialGuarantee> FinancialGuarantees
        {
            get { return FinancialGuaranteesCollection.ToSafeIEnumerable(); }
        }

        public FinancialGuarantee AddFinancialGuarantee(DateTime receivedDate)
        {
            var financialGuarantee = new FinancialGuarantee(receivedDate);

            FinancialGuaranteesCollection.Add(financialGuarantee);

            return financialGuarantee;
        }

        public FinancialGuarantee GetFinancialGuarantee(Guid financialGuaranteeId)
        {
            var financialGuarantee = FinancialGuarantees.SingleOrDefault(fg => fg.Id == financialGuaranteeId);

            if (financialGuarantee == null)
            {
                throw new InvalidOperationException(
                    string.Format("Financial guarantee with id {0} does not exist on this notification {1}", 
                        financialGuaranteeId, 
                        NotificationId));
            }

            return financialGuarantee;
        }

        public FinancialGuarantee GetLatestFinancialGuarantee()
        {
            return FinancialGuarantees.OrderByDescending(fg => fg.CreatedDate).FirstOrDefault();
        }
    }
}