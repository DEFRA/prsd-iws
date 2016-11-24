namespace EA.Iws.RequestHandlers.Tests.Unit.Admin.FinancialGuarantee
{
    using System;
    using Core.FinancialGuarantee;
    using DataAccess;
    using Domain.FinancialGuarantee;

    public class FinancialGuaranteeDecisionTests
    {
        protected static readonly DateTime FirstDate = new DateTime(2015, 1, 1);
        protected static readonly DateTime MiddleDate = new DateTime(2015, 1, 16);
        protected static readonly DateTime LastDate = new DateTime(2015, 2, 1);

        protected static readonly Guid ApplicationCompletedId = new Guid("9DA0EE37-EA13-4DF8-A4C9-0A8FDBC2207B");

        protected IwsContext context;

        protected class TestFinancialGuarantee : FinancialGuarantee
        {
            public bool ApproveCalled { get; private set; }

            public bool ApproveThrows { get; set; }

            public bool RejectThrows { get; set; }

            public bool RefuseCalled { get; set; }

            public bool RefuseThrows { get; set; }

            public bool ReleaseCalled { get; set; }

            public override void Approve(ApproveDates approveDates)
            {
                ApproveCalled = true;

                if (ApproveThrows)
                {
                    throw new InvalidOperationException();
                }

                Status = FinancialGuaranteeStatus.Approved;
            }

            public override void Refuse(DateTime decisionDate, string refusalReason)
            {
                RefuseCalled = true;

                if (RejectThrows)
                {
                    throw new InvalidOperationException();
                }

                Status = FinancialGuaranteeStatus.Refused;
            }

            public override void Release(DateTime decisionDate)
            {
                ReleaseCalled = true;

                if (RefuseThrows)
                {
                    throw new InvalidOperationException();
                }

                Status = FinancialGuaranteeStatus.Released;
            }
        }
    }
}