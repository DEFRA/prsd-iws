namespace EA.Iws.Domain.FinancialGuarantee
{
    using System;
    using Core.FinancialGuarantee;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class FinancialGuaranteeStatusChange : Entity
    {
        public FinancialGuaranteeStatus Status { get; private set; }

        public User User { get; private set; }

        public DateTimeOffset ChangeDate { get; private set; }

        protected FinancialGuaranteeStatusChange()
        {
        }

        public FinancialGuaranteeStatusChange(FinancialGuaranteeStatus status, User user)
        {
            Guard.ArgumentNotNull(() => user, user);

            User = user;
            Status = status;
            ChangeDate = new DateTimeOffset(SystemTime.UtcNow, TimeSpan.Zero);
        }
    }
}
