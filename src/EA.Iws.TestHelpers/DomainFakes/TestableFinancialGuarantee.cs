namespace EA.Iws.TestHelpers.DomainFakes
{
    using System;
    using Core.FinancialGuarantee;
    using Domain.FinancialGuarantee;
    using Helpers;

    public class TestableFinancialGuarantee : FinancialGuarantee
    {
        public new Guid Id
        {
            get { return base.Id; }
            set { ObjectInstantiator<FinancialGuarantee>.SetProperty(x => x.Id, value, this); }
        }

        public new DateTime? DecisionDate
        {
            get { return base.DecisionDate; }
            set { base.DecisionDate = value; }
        }

        public new string RefusalReason
        {
            get { return base.RefusalReason; }
            set { base.RefusalReason = value; }
        }

        public new int? ActiveLoadsPermitted
        {
            get { return base.ActiveLoadsPermitted; }
            set { base.ActiveLoadsPermitted = value; }
        }

        public new FinancialGuaranteeStatus Status
        {
            get { return base.Status; }
            set { base.Status = value; }
        }
    }
}
