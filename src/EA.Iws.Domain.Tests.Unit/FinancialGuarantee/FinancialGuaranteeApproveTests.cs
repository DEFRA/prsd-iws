namespace EA.Iws.Domain.Tests.Unit.FinancialGuarantee
{
    using System;
    using Core.FinancialGuarantee;
    using Domain.FinancialGuarantee;
    using Xunit;

    public class FinancialGuaranteeApproveTests : FinancialGuaranteeTests
    {
        private const int AnyInt = 7;
        private const string BlanketBondReference = "ref 23";

        private readonly Action<FinancialGuarantee> setGuaranteeApproved =
            fg =>
                fg.Approve(new ApprovalData(AfterCompletionDate, 
                    BlanketBondReference, 
                    AnyInt, false));

        private readonly Action<FinancialGuarantee> setGuaranteeApprovedIsBlanketBond =
            fg =>
                fg.Approve(new ApprovalData(AfterCompletionDate,
                    BlanketBondReference,
                    AnyInt, true));

        [Fact]
        public void StatusNotCompletedThrows()
        {
            Assert.Throws<InvalidOperationException>(() => setGuaranteeApproved(ReceivedFinancialGuarantee));
        }

        [Fact]
        public void DecisionDateBeforeCompletedDateThrows()
        {
            Assert.Throws<InvalidOperationException>(
                () =>
                    CompletedFinancialGuarantee.Approve(new ApprovalData(BeforeCompletionDate, BlanketBondReference, AnyInt, true)));
        }

        [Fact]
        public void SetsDecisionDate()
        {
            setGuaranteeApproved(CompletedFinancialGuarantee);

            Assert.Equal(CompletedDate.AddDays(1), CompletedFinancialGuarantee.DecisionDate);
        }

        [Fact]
        public void ChangesStatus()
        {
            setGuaranteeApproved(CompletedFinancialGuarantee);

            Assert.Equal(FinancialGuaranteeStatus.Approved, CompletedFinancialGuarantee.Status);
        }

        [Fact]
        public void SetsActiveLoadsPermitted()
        {
            setGuaranteeApproved(CompletedFinancialGuarantee);

            Assert.Equal(AnyInt, CompletedFinancialGuarantee.ActiveLoadsPermitted);
        }

        [Fact]
        public void DecisionIsApproved()
        {
            setGuaranteeApproved(CompletedFinancialGuarantee);

            Assert.Equal(FinancialGuaranteeDecision.Approved, CompletedFinancialGuarantee.Decision);
        }
    }
}
