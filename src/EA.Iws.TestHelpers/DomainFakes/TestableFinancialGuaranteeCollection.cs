namespace EA.Iws.TestHelpers.DomainFakes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.FinancialGuarantee;

    public class TestableFinancialGuaranteeCollection : FinancialGuaranteeCollection
    {
        public TestableFinancialGuaranteeCollection(Guid notificationId) : base(notificationId)
        {
        }

        public new IEnumerable<FinancialGuarantee> FinancialGuarantees
        {
            get { return base.FinancialGuarantees; }
            set { FinancialGuaranteesCollection = value.ToList(); }
        }
    }
}
